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
        DataManager.Instance.LoadAllItems();
        UIManager.Instance.Show<MainUI>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
