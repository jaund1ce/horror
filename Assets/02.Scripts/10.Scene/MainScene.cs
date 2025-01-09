using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
    // Start is called before the first frame update
    void Start()
    {
        InitiallizeFile();
        
    }

    private void InitiallizeFile()
    {
        //if(NewGame?)
        /*MapManager.Instance.ShowMap<Stage01>();
        MapManager.Instance.LoadAndSpawnObjects(1);
        DataManager.Instance.LoadAllItems();
        UIManager.Instance.Show<MainUI>();*/

        //if(LoadGame?)
        MapManager.Instance.ShowMap<Stage01>();
        DataManager.Instance.LoadAllItems();
        UIManager.Instance.Show<MainUI>();
        DataManager.Instance.LoadGame();
    }
}
