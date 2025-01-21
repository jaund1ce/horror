using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemBase : MonoBehaviour
{
    public bool OnUsing { get; protected set; }
    protected string animUse = "Use";
    protected InventoryData inventoryData;

    protected virtual void Start()
    {
        inventoryData = MainGameManager.Instance.Player.CurrentEquipItem;        
    }

    public virtual void OnUseInput() 
    {
        if (inventoryData == null) return;
        if (!OnUsing)
        {
            OnUsing = true;
            MainGameManager.Instance.Player.Animator.SetBool("OnUsing", OnUsing);
            Invoke("OnUse", 0.5f);
        }
    }

    public virtual void OnUse() 
    {
        OnUsing = false;
        MainGameManager.Instance.Player.Animator.SetBool("OnUsing", OnUsing);
        if (inventoryData.Use(1) == TryUse.ResetItem)
        {
            MainGameManager.Instance.Player.UnEquipCurrentItem();
        }
    }
}
