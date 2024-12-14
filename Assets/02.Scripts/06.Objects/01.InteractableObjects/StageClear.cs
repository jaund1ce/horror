using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{
    private LockedDoorWithHinge lockedDoorWithHinge;

    void Start()
    {
        lockedDoorWithHinge = GetComponent<LockedDoorWithHinge>();
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


