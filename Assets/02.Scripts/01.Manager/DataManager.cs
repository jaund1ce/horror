using System;
using System.Collections.Generic;
using UnityEngine;

public static class AssetType 
{
    public const string Prefab = "Prefab";
}

public static class CategoryType 
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Item = "Item";
    public const string Interactableobjects = "InteractableObjects";
}

public static class Json 
{
    public const string PlayerData = "PlayerData.json";
    public const string InventoryData = "InventoryData.json";
    public const string DropItemData = "DropItemData.json";
    public const string EnemyData = "EnemyData.json";
    public const string PaperData = "PaperData.json";
    public const string PuzzleData = "PuzzleData.json";
    public const string MapData = "MapData.json";
}

public class DataManager : mainSingleton<DataManager>
{
    public ItemData[] AllItems; // 게임 내 모든 아이템 데이터 배열
    public UserInfo PlayerData; // 플레이어 데이터
    public InventoryData[] InventoryData; // 인벤토리 데이터 배열
    public MapInfo MapData; // 맵 데이터
    InventorySlotsController InventorySlotsController;
    public Dictionary<string, SpawnData> SaveItemData;
    public Dictionary<string, SpawnData> SaveEnemyData;
    public Dictionary<string, SpawnData> SavePaperData;
    public Dictionary<string, bool> SavePuzzleData;

    protected override void Awake()
    {
        base.Awake();

        if (PlayerData == null) // MapData와 마찬가지로 public 으로 선언되었기때문에 유니티 자체에서 직렬화할때 초기화 해준다. 고로 타지 않음
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

        MapData = SaveSystem.Load<MapInfo>(Json.MapData) ?? new MapInfo();
        

        SaveItemData = new Dictionary<string, SpawnData>();
        SaveEnemyData = new Dictionary<string, SpawnData>();
        SavePaperData = new Dictionary<string, SpawnData>();
        SavePuzzleData = new Dictionary<string, bool>();
        
    }

/*    public void InitializeGameData()
    {
        PlayerData = new UserInfo();
        InventoryData = new InventoryData[16]; // 슬롯 수에 맞게 초기화
        for (int i = 0; i < InventoryData.Length; i++)
        {
            InventoryData[i] = new InventoryData(i);
        }
        MapData = new MapInfo();
    }*/

    public void LoadAllItems()
    {
        AllItems = Resources.LoadAll<ItemData>("SO/Item");

        if (AllItems == null || AllItems.Length == 0)
        {
            return;
        }

    }




    public void SaveGame(bool saveBtnClick = false)
    {

        SavePlayerData();

        // 직렬화 가능한 InventoryData 리스트 생성 (수정됨: 디버깅 추가)
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

        SaveSpawnData(saveBtnClick);
        

        MapData.MapName = Main_SceneManager.Instance.NowSceneName;

        // InventoryData 저장 (기존 코드 유지)
        SaveSystem.Save(serializableInventory, Json.InventoryData);

        // MapData 저장 (기존 코드 유지)
        SaveSystem.Save(MapData, "MapData.json");

        Debug.Log("Game Saved!"); // 게임 저장 완료 로그
    }


    public void LoadGame()
    {
        LoadPlayerData();

        SaveItemData = SaveSystem.Load<Dictionary<string, SpawnData>>(Json.DropItemData);
        SaveEnemyData = SaveSystem.Load<Dictionary<string, SpawnData>>(Json.EnemyData);
        SavePaperData = SaveSystem.Load<Dictionary<string, SpawnData>>(Json.PaperData);
        SavePuzzleData = SaveSystem.Load<Dictionary<string, bool>>(Json.PuzzleData);

        if (SaveEnemyData != null) // 저장시에 적이 없을경우는 없기에 최상위 if문으로 NewGame인지 LoadGame인지 검출중
        {
            LoadEnemyData();
            if (SaveItemData != null) LoadItemData();
            //## TODO :: 나중에 스폰 수정시에 주석해제
            //if (SavePaperData != null) LoadPaperData();
            if (SavePuzzleData != null) LoadPuzzleData();
        }
        else
        {
            int SceneNumber;
            if (string.IsNullOrEmpty(MapData.MapName)) SceneNumber = 1;
            else
            {
                SceneNumber = int.Parse(MapData.MapName[MapData.MapName.Length - 1].ToString());
            }
            MapManager.Instance.LoadAndSpawnObjects(SceneNumber);
        }
        MapData = SaveSystem.Load<MapInfo>(Json.MapData) ?? new MapInfo();

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
            spawndata.key = ItemBase.gameObject.name;
            spawndata.key = spawndata.key.Replace("(Clone)", "").Trim();
            spawndata.assetType = AssetType.Prefab;
            spawndata.categoryType = CategoryType.Item;
            string position = obj.transform.position.ToString();
            spawndata.position = position;
            spawndata.referenceObjectName = "";
            if (SaveItemData == null) SaveItemData = new Dictionary<string, SpawnData>();
            SaveItemData.Add(position, spawndata);
        }
    }

    //굳이 FindByItem 이랑 나눠놓은 이유는 , 후에 추가될 수도 있는 Enemy의 체력이나 기타조건이 있을수 있기때문
    //## TODO Enemy State , Enemy Direction
    public void FindByEnemy<T>(GameObject obj) where T : Enemy
    {
        if (obj.TryGetComponent<T>(out T Enemy))
        {
            SpawnData enemy = new SpawnData();
            enemy.key = Enemy.gameObject.name;
            enemy.key = enemy.key.Replace("(Clone)", "").Trim();
            enemy.assetType = AssetType.Prefab;
            enemy.categoryType = CategoryType.Enemy;
            string position = obj.transform.position.ToString();
            enemy.position = position;
            enemy.referenceObjectName = "";
            if (SaveEnemyData == null) SaveEnemyData = new Dictionary<string, SpawnData>();
            SaveEnemyData.Add(position, enemy);
        }
    }

    public void FindByPaper<T>(GameObject obj) where T : Paper
    {
        if (obj.TryGetComponent<T>(out T Paper))
        {
            SpawnData spawndata = new SpawnData();
            spawndata.key = Paper.gameObject.name;
            spawndata.key = spawndata.key.Replace("(Clone)", "").Trim();
            spawndata.assetType = AssetType.Prefab;
            spawndata.categoryType = CategoryType.Interactableobjects;
            string position = obj.transform.position.ToString();
            spawndata.position = position;
            spawndata.referenceObjectName = "";
            if (SaveItemData == null) SaveItemData = new Dictionary<string, SpawnData>();
            SavePaperData.Add(position, spawndata);
        }
    }

    public void FindByPuzzleBase<T>(GameObject obj) where T : PuzzleBase
    {
        if (obj.TryGetComponent<T>(out T PuzzleBase))
        {
            string prefabName = PuzzleBase.gameObject.name;
            bool isAccess = PuzzleBase.IsAccess;
            SavePuzzleData.Add(prefabName, isAccess);
        }
    }



/*    public void UpdateMapStatus()
    {
        MapData.ExploredAreas = new int[] { 1, 0, 1, 1 }; // 탐험된 지역
        Debug.Log("Map Updated!");
    }*/

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

    private void SavePlayerData() 
    {
        // MainGameManager의 데이터를 동기화
        PlayerData.PaperInteractionCount = MainGameManager.Instance.paperInteractionCount; // 추가
        PlayerData.Health = MainGameManager.Instance.Player.PlayerConditionController.Health;
        PlayerData.Stamina = MainGameManager.Instance.Player.PlayerConditionController.Stamina;
        PlayerData.Playerposition = (MainGameManager.Instance.Player.transform.position).ToString();

        // PlayerData 저장 (기존 코드 유지)
        SaveSystem.Save(PlayerData, Json.PlayerData);
    }

    private void SaveSpawnData(bool saveBtnClick)
    {

        if (saveBtnClick)
        {
            // 하이어라키 전체 탐색
            foreach (GameObject rootObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                // 그 중 SetActive(true) 로 된 오브젝트만 검색
                if (rootObject.activeSelf)
                {
                    FindByItemBase<ItemBase>(rootObject);
                    FindByEnemy<Enemy>(rootObject);
                    //## TODO :: 나중에 스폰 수정시에 주석해제
                    //FindByPaper<Paper>(rootObject);
                    FindByPuzzleBase<PuzzleBase>(rootObject);
                }
            }
            //저장하기 버튼을 눌렀을경우, 그 내용을 저장하고 배열을 비워준다. 비워주는 이유는 두번 저장하기 클릭시 두번 add됨
            SaveSystem.Save(SaveItemData, Json.DropItemData);
            SaveItemData.Clear();
            SaveSystem.Save(SaveEnemyData, Json.EnemyData);
            SaveEnemyData.Clear();
            //## TODO :: 나중에 스폰 수정시에 주석해제
            /*SaveSystem.Save(SavePaperData, "PaperData.json");
            SavePaperData.Clear();*/
            SaveSystem.Save(SavePuzzleData, Json.PuzzleData);
            SavePuzzleData.Clear();
        }
        else
        {
            //스테이지를 클리어하여 넘어갈때 데이터들을 저장하는데, DropItem 의 경우 다음스테이지에서 또 뿌려주면 안되기때문에
            // 먼저 세이브데이터를 비워주고 저장한다.
            SaveItemData.Clear();
            SaveSystem.Save(SaveItemData, Json.DropItemData);
            SaveEnemyData.Clear();
            SaveSystem.Save(SaveEnemyData, Json.EnemyData);
            /*SavePaperData.Clear();
            SaveSystem.Save(SavePaperData, "PaperData.json");*/
            SavePuzzleData.Clear();
            SaveSystem.Save(SavePuzzleData, Json.PuzzleData);
        }
    }

    private void LoadPlayerData() 
    {
        PlayerData = SaveSystem.Load<UserInfo>(Json.PlayerData);
        
        if (PlayerData != null)
        {
            SpawnData spawnPlayer = new SpawnData();
            spawnPlayer.key = PlayerData.PlayerName;
            spawnPlayer.assetType = AssetType.Prefab;
            spawnPlayer.categoryType = CategoryType.Player;
            spawnPlayer.position = PlayerData.Playerposition;
            MapManager.Instance.SpawnObject(spawnPlayer);
            Player player = MainGameManager.Instance.Player;
            MainGameManager.Instance.paperInteractionCount = PlayerData.PaperInteractionCount; // 추가
            player.PlayerConditionController.Health = PlayerData.Health;
            player.PlayerConditionController.Stamina = PlayerData.Stamina;
            
            Debug.Log("PlayerData loaded successfully.");
        }
        else
        {
            Debug.LogWarning("PlayerData not found, initializing new data.");
            PlayerData = new UserInfo();
        }

        var loadedInventory = SaveSystem.Load<List<InventoryData>>(Json.InventoryData);
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
    }

    private void LoadEnemyData() 
    {
        foreach (var enemyData in SaveEnemyData)
        {
            SpawnData spawndata = new SpawnData();
            spawndata.key = enemyData.Value.key;
            spawndata.assetType = enemyData.Value.assetType;
            spawndata.categoryType = enemyData.Value.categoryType;
            spawndata.position = enemyData.Key;
            spawndata.referenceObjectName = enemyData.Value.referenceObjectName;
            MapManager.Instance.SpawnObject(spawndata);
        }
        SaveEnemyData.Clear();
    }

    private void LoadItemData() 
    {
        foreach (var itemData in SaveItemData)
        {
            SpawnData spawndata = new SpawnData();
            spawndata.key = itemData.Value.key;
            spawndata.assetType = itemData.Value.assetType;
            spawndata.categoryType = itemData.Value.categoryType;
            spawndata.position = itemData.Key;
            spawndata.referenceObjectName = itemData.Value.referenceObjectName;
            MapManager.Instance.SpawnObject(spawndata);
        }
        SaveItemData.Clear();
    }

    private void LoadPaperData() 
    {
        foreach (var paperData in SavePaperData)
        {
            SpawnData spawndata = new SpawnData();
            spawndata.key = paperData.Value.key;
            spawndata.assetType = paperData.Value.assetType;
            spawndata.categoryType = paperData.Value.categoryType;
            spawndata.position = paperData.Key;
            spawndata.referenceObjectName = paperData.Value.referenceObjectName;
            MapManager.Instance.SpawnObject(spawndata);
        }
        SavePaperData.Clear();
    }

    private void LoadPuzzleData()
    {
        foreach (var puzzleData in SavePuzzleData)
        {
            GameObject puzzle = GameObject.Find(puzzleData.Key);
            PuzzleBase puzzleBase = puzzle.GetComponent<PuzzleBase>();
            if (puzzleBase != null) 
            {
                puzzleBase.IsAccess = puzzleData.Value;
                puzzleBase.Access(puzzleBase.IsAccess);
            }
        }
        SavePuzzleData.Clear();
    }
}
