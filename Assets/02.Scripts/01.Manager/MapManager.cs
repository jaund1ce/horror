using System.Collections.Generic;
using UnityEngine;


// JSON �����͸� �Ľ��ϰ� �ش� �����͸� ������� ������Ʈ�� �����ϴ� ��ũ��Ʈ
// SpawnData Ŭ����: JSON �����͸� ����ȭ�� �������� ����
[System.Serializable]
public class SpawnData
{
    public string key; // ResourceManager���� �ε��� �ڻ��� ���� Ű
    public string assetType; // eAssetType ������ �� (������, ����� �� �ڻ� ���� ����)
    public string categoryType; // eCategoryType ������ �� (������, NPC �� �ڻ� �з� ����)
    public Vector3 position; // ������Ʈ�� ������ ���� ��ǥ
    public string referenceObjectName; // Ư�� ������Ʈ �̸� (�� ������Ʈ�� ��ġ�� �������� ���� ��ġ ���)
}

// JSON ������ ������ �Ľ��ϱ� ���� ���� Ŭ����
[System.Serializable]
public class SpawnDataArrayWrapper
{
    public SpawnData[] data; // SpawnData �迭�� JSON �����͸� �Ľ�
}


public class MapManager : mainSingleton<MapManager>
{
    public static Transform MapTransform
    {
        get
        {
            if (mapTransform == null)
                mapTransform = FindFirstObjectByType<SceneBase>().mapTransform; // ���� Ȱ��ȭ�� SceneBase�� mapTransform ��������
            return mapTransform;
        }
        set { mapTransform = value; }
    }
    private static Transform mapTransform;
    public string jsonFilePath ="Data/SpawnData"; // JSON ���� ��� (Resources ���� ���� ����), ������ �ε��Ͽ� �����Ϳ� ���� ������Ʈ�� ����
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
        mapList.Add(mapName, map);
        return map.GetComponent<T>();
    }

    // �� �ε�
    private GameObject LoadMap(GameObject prefab, string mapName)
    {
        var newMapObject = Instantiate(prefab, MapTransform);
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
    public void LoadAndSpawnObjects()
    {
        // JSON ���� �ε�
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath); // Resources �������� JSON ���� �ε�
        if (jsonFile == null)
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�: {jsonFilePath}"); // JSON ������ ���� ��� ���� �޽��� ���
            return;
        }

        // JSON �����͸� SpawnData �迭�� �Ľ�
        SpawnData[] spawnDataArray = JsonUtility.FromJson<SpawnDataArrayWrapper>(jsonFile.text).data;

        // �� SpawnData �׸� ���� ������Ʈ�� ����
        foreach (var spawnData in spawnDataArray)
        {
            SpawnObject(spawnData); // ���� ������Ʈ ���� �Լ� ȣ��
        }
    }

    // ���� ������Ʈ�� �����ϴ� �Լ�
    private void SpawnObject(SpawnData data)
    {
        // assetType ���ڿ��� eAssetType ���������� ��ȯ
        if (!System.Enum.TryParse(data.assetType, out eAssetType assetType))
        {
            Debug.LogError($"��ȿ���� ���� assetType: {data.assetType}"); // �߸��� assetType�� ��� ���� �޽��� ���
            return;
        }

        // categoryType ���ڿ��� eCategoryType ���������� ��ȯ
        if (!System.Enum.TryParse(data.categoryType, out eCategoryType categoryType))
        {
            categoryType = eCategoryType.None; // ��ȯ ���� �� �⺻�� None���� ����
        }

        // ResourceManager�� ����� ������ �ε�
        GameObject prefab = ResourceManager.Instance.LoadAsset<GameObject>(data.key, assetType, categoryType);

        if (prefab == null)
        {
            Debug.LogError($"������ �ε� ����: {data.key}"); // ������ �ε� ���� �� ���� �޽��� ���
            return;
        }
        // ���� ������Ʈ�� ��ġ�� �����Ͽ� ���� ��ġ ����
        Vector3 spawnPosition = data.position;
        if (!string.IsNullOrEmpty(data.referenceObjectName))
        {
            GameObject referenceObject = GameObject.Find(data.referenceObjectName);
            if (referenceObject != null)
            {
                spawnPosition = referenceObject.transform.position + new Vector3(0, 2, 0); // y + 2���� ��ġ ����
            }
            else
            {
                Debug.LogWarning($"���� ������Ʈ�� ã�� �� �����ϴ�: {data.referenceObjectName}");
            }
        }

        // �ε�� �������� ���� (Instantiate �Լ� ���)
        Instantiate(prefab, spawnPosition, Quaternion.identity); // ������ ��ġ�� ������Ʈ ����
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}


