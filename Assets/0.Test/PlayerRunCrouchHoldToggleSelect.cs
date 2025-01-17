using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunCrouchHoldToggleSelect : MonoBehaviour
{
    public void OnChange()
    {
        //SoundManger.Instance.MakeEnviormentSound();
        //Debug.Log($"Hold : {isHold}");
    }
    public void ISHold()
    {
        MainGameManager.Instance.IsHold = true;
    }

    public void ISToggle()
    {
        MainGameManager.Instance.IsHold = false;
    }
}
