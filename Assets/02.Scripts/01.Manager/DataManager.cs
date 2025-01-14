using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
    public ItemData[] AllItems; // ���� �� ��� ������ ������ �迭
    public UserInfo PlayerData; // �÷��̾� ������
    public InventoryData[] InventoryData; // �κ��丮 ������ �迭
    public MapInfo MapData; // �� ������
    InventorySlotsController InventorySlotsController;
    public Dictionary<Vector3, int> SaveItemData;

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

        SaveItemData = new Dictionary<Vector3, int>();
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




    public void SaveGame()
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
                serializableInventory.Add(new InventoryData(inventorySlot.SlotIndex)
                {
                    ItemID = inventorySlot.ItemData.itemSO.ID,
                    Amount = inventorySlot.Amount,
                    QuickslotIndex = inventorySlot.QuickslotIndex
                });
            }
            
        }

        //foreach (GameObject rootObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) 
        //{
        //    FindByItemBase<ItemBase>(rootObject);
        //    FindByPaper<Paper>(rootObject);
        //}

        // InventoryData ���� (���� �ڵ� ����)
        SaveSystem.Save(serializableInventory, "InventoryData.json");

        // MapData ���� (���� �ڵ� ����)
        SaveSystem.Save(MapData, "MapData.json");

        SaveSystem.Save(SaveItemData, "DropItemData.json");

        Debug.Log("Game Saved!"); // ���� ���� �Ϸ� �α�
    }





    public void LoadGame()
    {
        PlayerData = SaveSystem.Load<UserInfo>("PlayerData.json");

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
                    InventoryData[serializedSlot.SlotIndex].SetItem(itemData, serializedSlot.Amount);
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

    //public void FindByItemBase<T>(GameObject obj) where T : ItemBase 
    //{
    //    if (obj.TryGetComponent<T>(out T ItemBase))
    //    {
    //        SaveItemData.Add(obj.transform.position, ItemBase.itemData.itemSO.ID);
    //    }
    //}

    //public void FindByPaper<T>(GameObject obj) where T : Paper
    //{
    //    if (obj.TryGetComponent<T>(out T Paper))
    //    {
    //        SaveItemData.Add(obj.transform.position, Paper.paperData.value);
    //    }
    //}



    public void UpdateMapStatus()
    {
        MapData.ExploredAreas = new int[] { 1, 0, 1, 1 }; // Ž��� ����
        Debug.Log("Map Updated!");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
