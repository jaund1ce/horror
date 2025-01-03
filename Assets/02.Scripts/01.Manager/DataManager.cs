using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
    public ItemData[] AllItems; // ���� �� ��� ������ ������ �迭
    public UserInfo PlayerData; // �÷��̾� ������
    public InventoryData[] InventoryData; // �κ��丮 ������ �迭
    public MapInfo MapData; // �� ������
    InventorySlotsController InventorySlotsController;


    protected override void Awake()
    {
        base.Awake();

        if (PlayerData == null)
        {
            PlayerData = new UserInfo();
        }

        if (InventoryData == null || InventoryData.Length != 15)
        {
            InventoryData = new InventoryData[15];
            for (int i = 0; i < InventoryData.Length; i++)
            {
                InventoryData[i] = new InventoryData(i);
            }
        }

        if (MapData == null)
        {
            MapData = new MapInfo();
        }
    }

    public void InitializeGameData()
    {
        PlayerData = new UserInfo();
        InventoryData = new InventoryData[15]; // ���� ���� �°� �ʱ�ȭ
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
            else
            {
                Debug.Log($"Slot {inventorySlot.slotIndex} is empty or null.");
            }
        }

        // InventoryData ���� (���� �ڵ� ����)
        SaveSystem.Save(serializableInventory, "InventoryData.json");

        // MapData ���� (���� �ڵ� ����)
        SaveSystem.Save(MapData, "MapData.json");

        Debug.Log("Game Saved!"); // ���� ���� �Ϸ� �α�
    }





    public void LoadGame()
    {
        PlayerData = SaveSystem.Load<UserInfo>("PlayerData.json");

        if (PlayerData != null)
        {
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
