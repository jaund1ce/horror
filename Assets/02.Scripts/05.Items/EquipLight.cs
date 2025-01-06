﻿using System;
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
        light = GetComponent<Light>();
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

        Invoke("OnUse", 2f);
        if (!OnUsing)
        {
            light.enabled = true;
            OnUsing = true;
        }
    }


    public override void OnUse()
    {
        base.OnUse();

        if (!OnUsing)
        {
            light.enabled = true;
            OnUsing = true;
        }
        else if (OnUsing)
        {
            light.enabled = false;
            OnUsing = false;
        }
    }
}

