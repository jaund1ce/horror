using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MainScene Start method called.");
        UIManager.Instance.Show<MainUI>();
        MapManager.Instance.LoadAndSpawnObjects();
        DataManager.Instance.LoadAllItems();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
