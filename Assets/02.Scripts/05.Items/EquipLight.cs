using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipLight : EquipItemBase
{
    private Light light;
    private bool usable;


    protected override void Start()
    {
        base.Start();
        light = MainGameManager.Instance.Player.gameObject.GetComponentInChildren<Light>();
        light.enabled = false;
    }

    public override void OnUseInput()
    {
        if (inventoryData == null) return;
        if (!usable) return;

        usable = false;
        Invoke("OnUse", 1f);
    }


    public override void OnUse()
    {
        usable = true;

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
    }
}

