using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
    public ItemData[] AllItems; // 게임 내 모든 아이템 데이터 배열
    public UserInfo PlayerData; // 플레이어 데이터
    public InventoryData[] InventoryData; // 인벤토리 데이터 배열
    public MapInfo MapData; // 맵 데이터
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
        InventoryData = new InventoryData[16]; // 슬롯 수에 맞게 초기화
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

        // MainGameManager의 데이터를 동기화
        PlayerData.paperInteractionCount = MainGameManager.Instance.paperInteractionCount; // 추가


        // PlayerData 저장 (기존 코드 유지)
        SaveSystem.Save(PlayerData, "PlayerData.json");

        // 직렬화 가능한 InventoryData 리스트 생성 (수정됨: 디버깅 추가)
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

        // InventoryData 저장 (기존 코드 유지)
        SaveSystem.Save(serializableInventory, "InventoryData.json");

        // MapData 저장 (기존 코드 유지)
        SaveSystem.Save(MapData, "MapData.json");

        SaveSystem.Save(SaveItemData, "DropItemData.json");

        Debug.Log("Game Saved!"); // 게임 저장 완료 로그
    }





    public void LoadGame()
    {
        PlayerData = SaveSystem.Load<UserInfo>("PlayerData.json");

        if (PlayerData != null)
        {
            MainGameManager.Instance.paperInteractionCount = PlayerData.paperInteractionCount; // 추가
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
        MapData.ExploredAreas = new int[] { 1, 0, 1, 1 }; // 탐험된 지역
        Debug.Log("Map Updated!");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
