using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear2 : MonoBehaviour
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
        SoundManger.Instance.ResetAllSounds();
        Main_SceneManager.Instance.LoadEndScene();
    }

}


