using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EquipKey : EquipItemBase
{
    protected override void Start()
    {
        base.Start();
    }

    public override void OnUseInput()
    {
        // key의 종류가 맞지 않으면 인풋이 들어오면 안됨
        // if(keyType != 정해진 타입) return;
        base.OnUseInput();
    }


    public override void OnUse() 
    {
        base.OnUse();
        //열쇠 작동 코드 작성
    }

}

