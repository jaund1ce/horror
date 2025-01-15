using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene2 : SceneBase
{
    // Start is called before the first frame update
    void Start()
    {
        MapManager.Instance.ShowMap<Stage02>();
        DataManager.Instance.LoadAllItems();
        UIManager.Instance.Show<MainUI>();
        DataManager.Instance.LoadGame();
    }
}
