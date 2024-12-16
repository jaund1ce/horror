using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemBase : MonoBehaviour
{
    protected bool onUsing;
    protected string animUse = "Use";
    protected InventoryData inventoryData;

    protected Animator animator;
    protected Camera camera;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
        inventoryData = MainGameManager.Instance.Player.CurrentEquipItem;
    }

    public virtual void OnUseInput() 
    {
        if (inventoryData == null) return;
        if (!onUsing)
        {
            ResetSlot();

            onUsing = true;
            inventoryData.amount -= 1;
            animator.SetTrigger(animUse);
        }
    }

    public virtual void OnUse() 
    {
        onUsing = false;
    }

    public void ResetSlot() 
    {
        if (inventoryData.amount <= 0)
        {
            inventoryData.ResetData();
            Destroy(this.gameObject);
            return;
        }
    }
}
