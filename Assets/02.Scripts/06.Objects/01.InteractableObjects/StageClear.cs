using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{
    private LockedDoor lockedDoorWithHinge;



    private void OnEnable()
    {
        lockedDoorWithHinge = GetComponent<LockedDoor>();
        lockedDoorWithHinge.isOpen += stageClear;
    }

    private void OnDisable()
    {
        lockedDoorWithHinge.isOpen -= stageClear;
    }

    private void stageClear()
    {
        DataManager.Instance.SaveGame();
        Main_SceneManager.Instance.LoadMainScene2();
    }

}


