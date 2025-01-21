using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUIRefreshRate : MonoBehaviour
{
    public void ChangeRefreshRate(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1280, 720, false);
                break;
            case 2:
                Screen.SetResolution(1850, 1000, false);
                break;
        }
    }
}
