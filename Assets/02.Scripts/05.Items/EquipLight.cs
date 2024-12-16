using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipLight : EquipItemBase
{
    private bool isLightOn;
    private Light light;


    protected override void Start()
    {
        base.Start();
        light = GetComponent<Light>();
        light.enabled = false;
        isLightOn = false;
    }

    private void Update()
    {
        if (isLightOn) 
        {
            //배터리 감소 로직 작성
            //if(배터리 0 이면) animator.SetBool(animUse,false);
        }
    }

    public override void OnUseInput()
    {
        if (inventoryData == null) return;
        if (!onUsing)
        {
            if (isLightOn) 
            {
                animator.SetBool(animUse, true);
            } else if (!isLightOn) 
            {
                animator.SetBool(animUse, false);
            }
            onUsing = true;
        }
    }


    public override void OnUse()
    {
        base.OnUse();

        if (isLightOn)
        {
            light.enabled = false;
            isLightOn = false;
            Debug.Log("isLightOff");
        }
        else if (!isLightOn)
        {
            light.enabled = true;
            isLightOn = true;
            Debug.Log("isLightOn");

        }
    }
}

