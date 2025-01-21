using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EquipBattery : EquipItemBase
{

    private bool usable = true;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnUseInput()
    {
        if (inventoryData == null) return;
        if (!usable) return;
        OnUsing = true;
        usable = false;
        SoundManger.Instance.MakeEnviornmentSound("BatteryChange");
        MainGameManager.Instance.Player.Animator.SetBool("OnUsing", OnUsing);
        Invoke("OnUse", 8f);
    }

    public override void OnUse()
    {
        MainGameManager.Instance.Player.PlayerConditionController.AddBatteryCapacity(inventoryData.ItemData.itemSO.ItemHealHealth);
        base.OnUse();
        usable = true;
        OnUsing = false;
    }

}
