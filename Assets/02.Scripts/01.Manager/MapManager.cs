using System.Collections.Generic;
using UnityEngine;


// JSON �����͸� �Ľ��ϰ� �ش� �����͸� ������� ������Ʈ�� �����ϴ� ��ũ��Ʈ
// SpawnData Ŭ����: JSON �����͸� ����ȭ�� �������� ����
[System.Serializable]
public class SpawnData
{
    public string Key; // ResourceManager���� �ε��� �ڻ��� ���� Ű
    public string AssetType; // eAssetType ������ �� (������, ����� �� �ڻ� ���� ����)
    public string CategoryType; // eCategoryType ������ �� (������, NPC �� �ڻ� �з� ����)
    public string Position; // ������Ʈ�� ������ ���� ��ǥ
    public string ReferenceObjectName; // Ư�� ������Ʈ �̸� (�� ������Ʈ�� ��ġ�� �������� ���� ��ġ ���)
    public int ID;
}

// JSON ������ ������ �Ľ��ϱ� ���� ���� Ŭ����
[System.Serializable]
public class SpawnDataArrayWrapper
{
    public SpawnData[] Data; // SpawnData �迭�� JSON �����͸� �Ľ�
}


public class MapManager : mainSingleton<MapManager>
{
    public string NowMapName = "";


    private Dictionary<string, GameObject> mapList = new Dictionary<string, GameObject>();
    


    protected override void Awake()
    {
        base.Awake();
    }


    // �� ǥ��
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
        NowMapName = mapName;
        mapList.Add(mapName, map);
        return map.GetComponent<T>();
    }

    // �� �ε�
    private GameObject LoadMap(GameObject prefab, string mapName)
    {
        var newMapObject = Instantiate(prefab);
        newMapObject.name = mapName;
        return newMapObject;
    }

    // �� �����
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

    // Null ���۷��� ����
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

    // JSON �����͸� �о�� ������Ʈ�� �����ϴ� �Լ�
    public void LoadAndSpawnObjects(int SceneNumber)
    {
        string path = $"Data/SpawnData{SceneNumber}";
        // JSON ���� �ε�
        TextAsset jsonFile = Resources.Load<TextAsset>(path); // Resources �������� JSON ���� �ε�
        if (jsonFile == null)
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�: {path}"); // JSON ������ ���� ��� ���� �޽��� ���
            return;
        }

        // JSON �����͸� SpawnData �迭�� �Ľ�
        SpawnData[] spawnDataArray = JsonUtility.FromJson<SpawnDataArrayWrapper>(jsonFile.text).Data;

        // �� SpawnData �׸� ���� ������Ʈ�� ����
        foreach (var spawnData in spawnDataArray)
        {
            SpawnObject(spawnData); // ���� ������Ʈ ���� �Լ� ȣ��
        }
    }
    public void LoadAndSpawnPapers(int SceneNumber)
    {
        string path = $"Data/PaperSpawnData{SceneNumber}";
        TextAsset jsonFile = Resources.Load<TextAsset>(path); 
        if (jsonFile == null)
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�: {path}"); 
            return;
        }
        SpawnData[] spawnDataArray = JsonUtility.FromJson<SpawnDataArrayWrapper>(jsonFile.text).Data;

        // �� SpawnData �׸� ���� ������Ʈ�� ����
        foreach (var spawnData in spawnDataArray)
        {
            GameObject prefab = ResourceManager.Instance.LoadAsset<GameObject>(spawnData.Key, eAssetType.Prefab, eCategoryType.Paper);
            if (prefab.TryGetComponent<Paper>(out Paper paper)) 
            {
                SpawnObject(spawnData , paper);
            }
        }
    }


    // ���� ������Ʈ�� �����ϴ� �Լ�
    public void SpawnObject(SpawnData data, Paper paper = null)
    {
        // assetType ���ڿ��� eAssetType ���������� ��ȯ
        if (!System.Enum.TryParse(data.AssetType, out eAssetType assetType))
        {
            Debug.LogError($"��ȿ���� ���� assetType: {data.AssetType}"); // �߸��� assetType�� ��� ���� �޽��� ���
            return;
        }

        // categoryType ���ڿ��� eCategoryType ���������� ��ȯ
        if (!System.Enum.TryParse(data.CategoryType, out eCategoryType categoryType))
        {
            categoryType = eCategoryType.None; // ��ȯ ���� �� �⺻�� None���� ����
        }

        // ResourceManager�� ����� ������ �ε�
        GameObject prefab = ResourceManager.Instance.LoadAsset<GameObject>(data.Key, assetType, categoryType);

        if (prefab == null)
        {
            Debug.LogError($"������ �ε� ����: {data.Key}"); // ������ �ε� ���� �� ���� �޽��� ���
            return;
        }
        // ���� ������Ʈ�� ��ġ�� �����Ͽ� ���� ��ġ ����
        Vector3 spawnPosition;
        if (!string.IsNullOrEmpty(data.Position))
        {
            spawnPosition = StringToVector3(data.Position);
        }
        else 
        {
            spawnPosition = Vector3.zero;
        }

        if (!string.IsNullOrEmpty(data.ReferenceObjectName))
        {
            GameObject referenceObject = GameObject.Find(data.ReferenceObjectName);
            if (referenceObject != null)
            {
                spawnPosition = referenceObject.transform.position + new Vector3(0, 2, 0); // y + 2���� ��ġ ����
            }
            else
            {
                Debug.LogWarning($"���� ������Ʈ�� ã�� �� �����ϴ�: {data.ReferenceObjectName}");
            }
        }

        // �ε�� �������� ���� (Instantiate �Լ� ���)
        GameObject spawnObject = Instantiate(prefab, spawnPosition, prefab.transform.rotation); // ������ ��ġ�� ������Ʈ ����

        if (spawnObject.TryGetComponent<Paper>(out paper))
        {
            paper.PaperID = data.ID;
        }
    }



    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private Vector3 StringToVector3(string s)
    {
        s = s.Trim('(', ')');
        string[] parts = s.Split(',');
        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2])
        );
    }
}


