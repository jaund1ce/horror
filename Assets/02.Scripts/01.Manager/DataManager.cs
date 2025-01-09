using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
    public ItemData[] AllItems; // ���� �� ��� ������ ������ �迭
    public UserInfo PlayerData; // �÷��̾� ������
    public InventoryData[] InventoryData; // �κ��丮 ������ �迭
    public MapInfo MapData; // �� ������
    InventorySlotsController InventorySlotsController;
    public Dictionary<string, SpawnData> SaveItemData;

    protected override void Awake()
    {
        base.Awake();

        if (PlayerData == null)
        {
            PlayerData = new UserInfo();
        }

        if (InventoryData == null || InventoryData.Length != 15)
        {
            InventoryData = new InventoryData[16];
            for (int i = 0; i < InventoryData.Length; i++)
            {
                InventoryData[i] = new InventoryData(i);
            }
        }

        if (MapData == null)
        {
            MapData = new MapInfo();
        }

        SaveItemData = new Dictionary<string, SpawnData>();
    }

    public void InitializeGameData()
    {
        PlayerData = new UserInfo();
        InventoryData = new InventoryData[16]; // ���� ���� �°� �ʱ�ȭ
        for (int i = 0; i < InventoryData.Length; i++)
        {
            InventoryData[i] = new InventoryData(i);
        }
        MapData = new MapInfo();
    }

    public void LoadAllItems()
    {
        AllItems = Resources.LoadAll<ItemData>("SO/Item");

        if (AllItems == null || AllItems.Length == 0)
        {
            return;
        }

    }




    public void SaveGame(bool saveBtnClick)
    {

        // MainGameManager�� �����͸� ����ȭ
        PlayerData.paperInteractionCount = MainGameManager.Instance.paperInteractionCount; // �߰�

        // PlayerData ���� (���� �ڵ� ����)
        SaveSystem.Save(PlayerData, "PlayerData.json");

        // ����ȭ ������ InventoryData ����Ʈ ���� (������: ����� �߰�)
        var serializableInventory = new List<InventoryData>();
        foreach (var inventorySlot in InventoryData)
        {
            if (inventorySlot.ItemData != null)
            {
                serializableInventory.Add(new InventoryData(inventorySlot.slotIndex)
                {
                    ItemID = inventorySlot.ItemData.itemSO.ID,
                    amount = inventorySlot.amount,
                    quickslotIndex = inventorySlot.quickslotIndex
                });
            }
            
        }

        if (saveBtnClick)
        {
            foreach (GameObject rootObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                FindByItemBase<ItemBase>(rootObject);
                //FindByPaper<Paper>(rootObject);
            }
            SaveSystem.Save(SaveItemData, "DropItemData.json");
        }
        else 
        {
            SaveItemData.Clear();
            SaveSystem.Save(SaveItemData, "DropItemData.json");
        }

        MapData.MapName = Main_SceneManager.Instance.NowSceneName;

        // InventoryData ���� (���� �ڵ� ����)
        SaveSystem.Save(serializableInventory, "InventoryData.json");

        // MapData ���� (���� �ڵ� ����)
        SaveSystem.Save(MapData, "MapData.json");

        Debug.Log("Game Saved!"); // ���� ���� �Ϸ� �α�
    }





    public void LoadGame()
    {
        PlayerData = SaveSystem.Load<UserInfo>("PlayerData.json");
        SaveItemData = SaveSystem.Load<Dictionary<string, SpawnData>>("DropItemData.json");


        if (PlayerData != null)
        {
            MainGameManager.Instance.paperInteractionCount = PlayerData.paperInteractionCount; // �߰�
            Debug.Log("PlayerData loaded successfully.");
        }
        else
        {
            Debug.LogWarning("PlayerData not found, initializing new data.");
            PlayerData = new UserInfo();
        }

        var loadedInventory = SaveSystem.Load<List<InventoryData>>("InventoryData.json");
        if (loadedInventory != null)
        {
            Debug.Log($"Loaded {loadedInventory.Count} inventory slots.");
            foreach (var serializedSlot in loadedInventory)
            {
                var itemData = FindItemByID(serializedSlot.ItemID);
                if (itemData != null)
                {
                    InventoryData[serializedSlot.slotIndex].SetItem(itemData, serializedSlot.amount);
                }
                else
                {
                    Debug.LogWarning($"Item with ID {serializedSlot.ItemID} not found.");
                }
            }
        }
        else
        {
            Debug.LogWarning("InventoryData.json not found, initializing empty inventory.");
        }

        if (SaveItemData != null)
        {
            foreach (var serializedItemData in SaveItemData)
            {
                SpawnData spawndata = new SpawnData();
                spawndata.key = serializedItemData.Value.key;
                spawndata.assetType = serializedItemData.Value.assetType;
                spawndata.categoryType = serializedItemData.Value.categoryType;
                spawndata.position = StringToVector3(serializedItemData.Key);
                spawndata.referenceObjectName = serializedItemData.Value.referenceObjectName;
                MapManager.Instance.SpawnObject(spawndata);
            }

        }
        else
        {
            int SceneNumber;
            if (string.IsNullOrEmpty(MapData.MapName))
            {
                SceneNumber = 1;
            }
            else
            {
                SceneNumber = int.Parse(MapData.MapName[MapData.MapName.Length-1].ToString());
            }

            MapManager.Instance.LoadAndSpawnObjects(SceneNumber);
        }


        MapData = SaveSystem.Load<MapInfo>("MapData.json") ?? new MapInfo();
        


        Debug.Log("Game Loaded!");
    }




    public ItemData FindItemByID(int id)
    {
        foreach (var item in AllItems)
        {
            if (item.itemSO.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public void FindByItemBase<T>(GameObject obj) where T : ItemBase 
    {
        if (obj.TryGetComponent<T>(out T ItemBase))
        {
            SpawnData spawndata = new SpawnData();
            spawndata.key = ItemBase.itemData.name;
            spawndata.assetType = "Prefab";
            spawndata.categoryType = "Item";
            spawndata.position = obj.transform.position;
            spawndata.referenceObjectName = "";
            string key = obj.transform.position.ToString();
            if (SaveItemData == null) SaveItemData = new Dictionary<string, SpawnData>();
            SaveItemData.Add(key, spawndata);
        }
    }

    public void FindByPaper<T>(GameObject obj) where T : Paper
    {
        if (obj.TryGetComponent<T>(out T Paper))
        {
           // SaveItemData.Add(obj.transform.position, Paper.paperData.value);
        }
    }



    public void UpdateMapStatus()
    {
        MapData.ExploredAreas = new int[] { 1, 0, 1, 1 }; // Ž��� ����
        Debug.Log("Map Updated!");
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
