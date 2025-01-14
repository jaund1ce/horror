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
        DataManager.Instance.SaveGame(false);
        Main_SceneManager.Instance.LoadGame();
    }

}


