using System.Collections.Generic;
using UnityEngine;


// JSON 데이터를 파싱하고 해당 데이터를 기반으로 오브젝트를 스폰하는 스크립트
// SpawnData 클래스: JSON 데이터를 구조화된 형식으로 정의
[System.Serializable]
public class SpawnData
{
    public string key; // ResourceManager에서 로드할 자산의 고유 키
    public string assetType; // eAssetType 열거형 값 (프리팹, 썸네일 등 자산 유형 지정)
    public string categoryType; // eCategoryType 열거형 값 (아이템, NPC 등 자산 분류 지정)
    public Vector3 position; // 오브젝트가 스폰될 월드 좌표
    public string referenceObjectName; // 특정 오브젝트 이름 (이 오브젝트의 위치를 기준으로 스폰 위치 계산)
}

// JSON 데이터 구조를 파싱하기 위한 래퍼 클래스
[System.Serializable]
public class SpawnDataArrayWrapper
{
    public SpawnData[] data; // SpawnData 배열로 JSON 데이터를 파싱
}


public class MapManager : mainSingleton<MapManager>
{
    public static Transform MapTransform
    {
        get
        {
            if (mapTransform == null)
                mapTransform = FindFirstObjectByType<SceneBase>().mapTransform; // 현재 활성화된 SceneBase의 mapTransform 가져오기
            return mapTransform;
        }
        set { mapTransform = value; }
    }
    private static Transform mapTransform;
    public string jsonFilePath ="Data/SpawnData"; // JSON 파일 경로 (Resources 폴더 내부 기준), 파일을 로드하여 데이터에 따라 오브젝트를 스폰
    private Dictionary<string, GameObject> mapList = new Dictionary<string, GameObject>();


    protected override void Awake()
    {
        base.Awake();
    }


    // 맵 표시
    public T ShowMap<T>() where T : BaseMap
    {
        RemoveNull();
        string mapName = typeof(T).ToString();

        if (mapList.TryGetValue(mapName, out GameObject existingMap))
        {
            Debug.LogWarning($"{mapName} is already active.");
            return existingMap.GetComponent<T>();
        }

        var prefab = ResourceManager.Instance.LoadAsset<GameObject>(mapName, eAssetType.Prefab, eCategoryType.Maps);
        if (prefab == null)
        {
            Debug.LogError($"Map prefab {mapName} not found.");
            return null;
        }

        var map = LoadMap(prefab, mapName);
        mapList.Add(mapName, map);
        return map.GetComponent<T>();
    }

    // 맵 로드
    private GameObject LoadMap(GameObject prefab, string mapName)
    {
        var newMapObject = Instantiate(prefab, MapTransform);
        newMapObject.name = mapName;
        return newMapObject;
    }

    // 맵 숨기기
    public void HideMap<T>() where T : BaseMap
    {
        string mapName = typeof(T).ToString();
        HideMap(mapName);
    }

    public void HideMap(string mapName)
    {
        if (mapList.TryGetValue(mapName, out GameObject map))
        {
            Destroy(map);
            mapList.Remove(mapName);
        }
        else
        {
            Debug.LogWarning($"Map {mapName} not found in mapList.");
        }
    }

    // Null 레퍼런스 제거
    private void RemoveNull()
    {
        List<string> tempList = new List<string>(mapList.Count);
        foreach (var kvp in mapList)
        {
            if (kvp.Value == null)
                tempList.Add(kvp.Key);
        }

        foreach (var mapName in tempList)
        {
            mapList.Remove(mapName);
        }
    }

    // JSON 데이터를 읽어와 오브젝트를 스폰하는 함수
    public void LoadAndSpawnObjects()
    {
        // JSON 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath); // Resources 폴더에서 JSON 파일 로드
        if (jsonFile == null)
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: {jsonFilePath}"); // JSON 파일이 없는 경우 오류 메시지 출력
            return;
        }

        // JSON 데이터를 SpawnData 배열로 파싱
        SpawnData[] spawnDataArray = JsonUtility.FromJson<SpawnDataArrayWrapper>(jsonFile.text).data;

        // 각 SpawnData 항목에 대해 오브젝트를 스폰
        foreach (var spawnData in spawnDataArray)
        {
            SpawnObject(spawnData); // 개별 오브젝트 스폰 함수 호출
        }
    }

    // 개별 오브젝트를 스폰하는 함수
    private void SpawnObject(SpawnData data)
    {
        // assetType 문자열을 eAssetType 열거형으로 변환
        if (!System.Enum.TryParse(data.assetType, out eAssetType assetType))
        {
            Debug.LogError($"유효하지 않은 assetType: {data.assetType}"); // 잘못된 assetType의 경우 오류 메시지 출력
            return;
        }

        // categoryType 문자열을 eCategoryType 열거형으로 변환
        if (!System.Enum.TryParse(data.categoryType, out eCategoryType categoryType))
        {
            categoryType = eCategoryType.None; // 변환 실패 시 기본값 None으로 설정
        }

        // ResourceManager를 사용해 프리팹 로드
        GameObject prefab = ResourceManager.Instance.LoadAsset<GameObject>(data.key, assetType, categoryType);

        if (prefab == null)
        {
            Debug.LogError($"프리팹 로드 실패: {data.key}"); // 프리팹 로드 실패 시 오류 메시지 출력
            return;
        }
        // 기준 오브젝트의 위치를 참조하여 스폰 위치 조정
        Vector3 spawnPosition = data.position;
        if (!string.IsNullOrEmpty(data.referenceObjectName))
        {
            GameObject referenceObject = GameObject.Find(data.referenceObjectName);
            if (referenceObject != null)
            {
                spawnPosition = referenceObject.transform.position + new Vector3(0, 2, 0); // y + 2으로 위치 조정
            }
            else
            {
                Debug.LogWarning($"기준 오브젝트를 찾을 수 없습니다: {data.referenceObjectName}");
            }
        }

        // 로드된 프리팹을 스폰 (Instantiate 함수 사용)
        Instantiate(prefab, spawnPosition, Quaternion.identity); // 지정된 위치에 오브젝트 생성
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}


