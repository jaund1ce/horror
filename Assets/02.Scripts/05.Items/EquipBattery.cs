using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipBattery : EquipItemBase
{

    protected override void Start()
    {
        base.Start();
    }

    public override void OnUseInput()
    {
        base.OnUseInput();
    }


    public override void OnUse()
    {
        MainGameManager.Instance.Player.PlayerConditionController.AddBatteryCapacity(inventoryData.ItemData.itemSO.ItemHealHealth);
        SoundManger.Instance.MakeEnviornmentSound("BatteryChange");
        base.OnUse();
    }

}
