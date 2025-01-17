using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{
    private LockedDoor lockedDoorWithHinge;



    private void OnEnable()
    {
        lockedDoorWithHinge = GetComponent<LockedDoor>();
        lockedDoorWithHinge.isOpen += StageClearEvent;
    }

    private void OnDisable()
    {
        lockedDoorWithHinge.isOpen -= StageClearEvent;
    }

    private void StageClearEvent()
    {
        string sceneName = Main_SceneManager.Instance.NowSceneName;
        Main_SceneManager.Instance.NowSceneName = ConvertNumber(sceneName);
        string mapName =  MapManager.Instance.NowMapName;
        MapManager.Instance.NowMapName = ConvertNumber(mapName);
        DataManager.Instance.SaveGame(false);
        Main_SceneManager.Instance.LoadGame();
    }

    private string ConvertNumber(string name) 
    {
        int sceneNumber = int.Parse(name.Substring(name.Length -1));
        sceneNumber++;
        name = name.Substring(0, name.Length - 1);
        name = name + $"{sceneNumber}";
        return name;
    }

}


