using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUISceenResolutionChange : MonoBehaviour
{
    public void OnSceenValueChange(int index)//순서대로 0,1,2
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 1:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case 2:
                Screen.SetResolution(720, 480, Screen.fullScreenMode);
                break;
            default: Debug.Log("index error"); break;
        }
    }

    public void OnScreenTypeChange(int index)//순서대로 0,1,2
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default: Debug.Log("index error"); break;
        }
    }
}
