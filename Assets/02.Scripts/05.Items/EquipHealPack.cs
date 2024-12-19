using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipHealPack : EquipItemBase
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
        base.OnUse();
        MainGameManager.Instance.Player.playerConditionController.AddHealth(inventoryData.ItemData.itemSO.ItemHealHealth);
        ResetSlot();
    }

}
