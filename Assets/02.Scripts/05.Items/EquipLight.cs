using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EquipLight : EquipItemBase
{
    private Light light;
    private bool usable = false;

    protected override void Start()
    {
        base.Start();
        light = MainGameManager.Instance.Player.gameObject.GetComponentInChildren<Light>();
        ChangeLightIntencity();
        light.enabled = false;
    }

    private void ChangeLightIntencity()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScene")
        {
            light.intensity = 10;
        }
        else if (sceneName == "MainScene2")
        {
            light.intensity = 200;
        }
    }

    public override void OnUseInput()
    {
        if (inventoryData == null) return;
        if (!usable) return;

        usable = false;
        Invoke("OnUse", 0.5f);
    }


    public override void OnUse()
    {
        if (!OnUsing)
        {
            SoundManger.Instance.MakeEnviormentSound("Flashlight_On");
            light.enabled = true;
            OnUsing = true;
        }
        else
        {
            SoundManger.Instance.MakeEnviormentSound("Flashlight_Off");
            light.enabled = false;
            OnUsing = false;
        }
        usable = true;
    }
}

