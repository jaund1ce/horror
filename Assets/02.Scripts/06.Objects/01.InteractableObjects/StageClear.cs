using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{
    private LockedDoor lockedDoorWithHinge;

    void Start()
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
        Main_SceneManager.Instance.LoadEndScene();
    }

}


