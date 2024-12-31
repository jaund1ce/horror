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
            Debug.Log("PlayerData initialized.");
        }

        if (InventoryData == null)
        {
            InventoryData = new InventoryData[15];
            for (int i = 0; i < InventoryData.Length; i++)
            {
                InventoryData[i] = new InventoryData(i);
            }
            Debug.Log("InventoryData initialized.");
        }

        if (MapData == null)
        {
            MapData = new MapInfo();
            Debug.Log("MapData initialized.");
        }
    }

    public void InitializeGameData()
    {
        Debug.Log("Initializing game data...");
        PlayerData = new UserInfo();
        InventoryData = new InventoryData[15]; // ���� ���� �°� �ʱ�ȭ
        for (int i = 0; i < InventoryData.Length; i++)
        {
            InventoryData[i] = new InventoryData(i);
            Debug.Log($"Initialized slot {i}");
        }
        MapData = new MapInfo();
    }

    public void LoadAllItems()
    {
        Debug.Log("Entering LoadAllItems...");
        AllItems = Resources.LoadAll<ItemData>("SO/Item");

        if (AllItems == null || AllItems.Length == 0)
        {
            Debug.LogWarning("No items found in Resources/SO/Item!");
            return;
        }

        Debug.Log($"Loaded {AllItems.Length} items.");
        foreach (var item in AllItems)
        {
            Debug.Log($"Item Loaded: {item.itemSO.ItemNameEng}, ID: {item.itemSO.ID}");
        }
    }




    public void SaveGame()
    {
        Debug.Log("Saving game...");

        // �������� ���� ���� �ڵ�: InventoryData ���� Ȯ��
        foreach (var slot in InventoryData)
        {
            if (slot.ItemData != null)
            {
                Debug.Log($"Slot {slot.slotIndex}: ItemID={slot.ItemData.itemSO.ID}, Amount={slot.amount}");
            }
            else
            {
                Debug.Log($"Slot {slot.slotIndex}: Empty");
            }
        }

        // PlayerData ���� (���� �ڵ� ����)
        SaveSystem.Save(PlayerData, "PlayerData.json");

        // ����ȭ ������ InventoryData ����Ʈ ���� (������: ����� �߰�)
        var serializableInventory = new List<InventoryData>();
        foreach (var inventorySlot in InventoryData)
        {
            if (inventorySlot.ItemData != null)
            {
                Debug.Log($"Adding Slot {inventorySlot.slotIndex} to serializable inventory: ItemID={inventorySlot.ItemID}, Amount={inventorySlot.amount}");
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

        Debug.Log($"Saving {serializableInventory.Count} inventory slots."); // ����ȭ�� ���� ���� Ȯ��

        // InventoryData ���� (���� �ڵ� ����)
        SaveSystem.Save(serializableInventory, "InventoryData.json");

        // MapData ���� (���� �ڵ� ����)
        SaveSystem.Save(MapData, "MapData.json");

        Debug.Log("Game Saved!"); // ���� ���� �Ϸ� �α�
    }





    public void LoadGame()
    {
        Debug.Log("Loading game...");
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
                    Debug.Log($"Slot {serializedSlot.slotIndex} Loaded: ItemID={serializedSlot.ItemID}, Amount={serializedSlot.amount}");
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
        Debug.Log($"Searching for item with ID: {id}");
        foreach (var item in AllItems)
        {
            Debug.Log($"Checking item: {item.itemSO.ItemNameEng}, ID: {item.itemSO.ID}");
            if (item.itemSO.ID == id)
            {
                Debug.Log($"Item found: {item.itemSO.ItemNameEng}, ID: {item.itemSO.ID}");
                return item;
            }
        }
        Debug.LogWarning($"Item with ID {id} not found.");
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
