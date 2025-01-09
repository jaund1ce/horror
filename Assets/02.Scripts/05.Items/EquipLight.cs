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


    protected override void Start()
    {
        base.Start();
        light = MainGameManager.Instance.Player.gameObject.GetComponentInChildren<Light>();
        light.enabled = false;
    }

    private void Update()
    {
        if (OnUsing) 
        {
            //배터리 감소 로직 작성
            //if(배터리 0 이면) animator.SetBool(animUse,false);
        }
    }

    public override void OnUseInput()
    {
        if (inventoryData == null) return;

        Invoke("OnUse", 1f);
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
    }
}

