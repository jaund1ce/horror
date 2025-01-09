using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
    // Start is called before the first frame update
    void Start()
    {
        MapManager.Instance.ShowMap<Stage01>();
        MapManager.Instance.LoadAndSpawnObjects();
        UIManager.Instance.Show<MainUI>();
        DataManager.Instance.LoadAllItems();
    }
}
