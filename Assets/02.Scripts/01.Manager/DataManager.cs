using System;
using System.Collections.Generic;
using UnityEngine;
using static UHFPS.Runtime.InventoryItem;

public class DataManager : mainSingleton<DataManager>
{
    public ItemData[] AllItems; // ���� �� ��� ������ ������ �迭
    public UserInfo PlayerData; // �÷��̾� ������
    public InventoryData[] InventoryData; // �κ��丮 ������ �迭
    public MapInfo MapData; // �� ������
    InventorySlotsController InventorySlotsController;
    public Dictionary<string, SpawnData> SaveItemData;
    public Dictionary<string, SpawnData> SaveEnemyData;
    public Dictionary<string, SpawnData> SavePaperData;
    public Dictionary<string, bool> SavePuzzleData;

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
        SaveEnemyData = new Dictionary<string, SpawnData>();
        SavePaperData = new Dictionary<string, SpawnData>();
        SavePuzzleData = new Dictionary<string, bool>();
    }

/*    public void InitializeGameData()
    {
        PlayerData = new UserInfo();
        InventoryData = new InventoryData[16]; // ���� ���� �°� �ʱ�ȭ
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




    public void SaveGame(bool saveBtnClick)
    {

        SavePlayerData();

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

        SaveSpawnData(saveBtnClick);
        

        MapData.MapName = Main_SceneManager.Instance.NowSceneName;

        // InventoryData ���� (���� �ڵ� ����)
        SaveSystem.Save(serializableInventory, "InventoryData.json");

        // MapData ���� (���� �ڵ� ����)
        SaveSystem.Save(MapData, "MapData.json");

        Debug.Log("Game Saved!"); // ���� ���� �Ϸ� �α�
    }


    public void LoadGame()
    {
        LoadPlayerData();

        SaveItemData = SaveSystem.Load<Dictionary<string, SpawnData>>("DropItemData.json");
        SaveEnemyData = SaveSystem.Load<Dictionary<string, SpawnData>>("EnemyData.json");
        SavePaperData = SaveSystem.Load<Dictionary<string, SpawnData>>("PaperData.json");
        SavePuzzleData = SaveSystem.Load<Dictionary<string, bool>>("PuzzleData.json");

        if (SaveEnemyData != null) // ����ÿ� ���� �������� ���⿡ �ֻ��� if������ NewGame���� LoadGame���� ������
        {
            LoadEnemyData();
            if (SaveItemData != null) LoadItemData();
            //## TODO :: ���߿� ���� �����ÿ� �ּ�����
            //if (SavePaperData != null) LoadPaperData();
            if (SavePuzzleData != null) LoadPuzzleData();
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
                SceneNumber = int.Parse(MapData.MapName[MapData.MapName.Length - 1].ToString());
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
            spawndata.key = ItemBase.gameObject.name;
            spawndata.key = spawndata.key.Replace("(Clone)", "").Trim();
            spawndata.assetType = "Prefab";
            spawndata.categoryType = "Item";
            string position = obj.transform.position.ToString();
            spawndata.position = position;
            spawndata.referenceObjectName = "";
            if (SaveItemData == null) SaveItemData = new Dictionary<string, SpawnData>();
            SaveItemData.Add(position, spawndata);
        }
    }

    //���� FindByItem �̶� �������� ������ , �Ŀ� �߰��� ���� �ִ� Enemy�� ü���̳� ��Ÿ������ ������ �ֱ⶧��
    //## TODO Enemy State , Enemy Direction
    public void FindByEnemy<T>(GameObject obj) where T : Enemy
    {
        if (obj.TryGetComponent<T>(out T Enemy))
        {
            SpawnData enemy = new SpawnData();
            enemy.key = Enemy.gameObject.name;
            enemy.key = enemy.key.Replace("(Clone)", "").Trim();
            enemy.assetType = "Prefab";
            enemy.categoryType = "Enemy";
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
            spawndata.assetType = "Prefab";
            spawndata.categoryType = "InteractableObjects";
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
        MapData.ExploredAreas = new int[] { 1, 0, 1, 1 }; // Ž��� ����
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
        // MainGameManager�� �����͸� ����ȭ
        PlayerData.PaperInteractionCount = MainGameManager.Instance.paperInteractionCount; // �߰�
        PlayerData.Health = MainGameManager.Instance.Player.PlayerConditionController.Health;
        PlayerData.Stamina = MainGameManager.Instance.Player.PlayerConditionController.Stamina;
        PlayerData.Playerposition = (MainGameManager.Instance.Player.transform.position).ToString();

        // PlayerData ���� (���� �ڵ� ����)
        SaveSystem.Save(PlayerData, "PlayerData.json");
    }

    private void SaveSpawnData(bool saveBtnClick)
    {
        if (saveBtnClick)
        {
            // ���̾��Ű ��ü Ž��
            foreach (GameObject rootObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                // �� �� SetActive(true) �� �� ������Ʈ�� �˻�
                if (rootObject.activeSelf)
                {
                    FindByItemBase<ItemBase>(rootObject);
                    FindByEnemy<Enemy>(rootObject);
                    //## TODO :: ���߿� ���� �����ÿ� �ּ�����
                    //FindByPaper<Paper>(rootObject);
                    FindByPuzzleBase<PuzzleBase>(rootObject);
                }
            }
            //�����ϱ� ��ư�� ���������, �� ������ �����ϰ� �迭�� ����ش�. ����ִ� ������ �ι� �����ϱ� Ŭ���� �ι� add��
            SaveSystem.Save(SaveItemData, "DropItemData.json");
            SaveItemData.Clear();
            SaveSystem.Save(SaveEnemyData, "EnemyData.json");
            SaveEnemyData.Clear();
            //## TODO :: ���߿� ���� �����ÿ� �ּ�����
            /*SaveSystem.Save(SavePaperData, "PaperData.json");
            SavePaperData.Clear();*/
            SaveSystem.Save(SavePuzzleData, "PuzzleData.json");
            SavePuzzleData.Clear();
        }
        else
        {
            //���������� Ŭ�����Ͽ� �Ѿ�� �����͵��� �����ϴµ�, DropItem �� ��� ���������������� �� �ѷ��ָ� �ȵǱ⶧����
            // ���� ���̺굥���͸� ����ְ� �����Ѵ�.
            SaveItemData.Clear();
            SaveSystem.Save(SaveItemData, "DropItemData.json");
            SaveEnemyData.Clear();
            SaveSystem.Save(SaveEnemyData, "EnemyData.json");
            /*SavePaperData.Clear();
            SaveSystem.Save(SavePaperData, "PaperData.json");*/
            SavePuzzleData.Clear();
            SaveSystem.Save(SavePuzzleData, "PuzzleData.json");
        }
    }

    private void LoadPlayerData() 
    {
        PlayerData = SaveSystem.Load<UserInfo>("PlayerData.json");
        
        if (PlayerData != null)
        {
            Player player = MainGameManager.Instance.Player;
            MainGameManager.Instance.paperInteractionCount = PlayerData.PaperInteractionCount; // �߰�
            player.PlayerConditionController.Health = PlayerData.Health;
            player.PlayerConditionController.Stamina = PlayerData.Stamina;
            player.transform.position = StringToVector3(PlayerData.Playerposition);
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
    }
}
