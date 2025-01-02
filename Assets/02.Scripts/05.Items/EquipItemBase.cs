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
            onUsing = true;
            if (inventoryData.Use(1) == (int)TryUse.ResetItem) 
            {
                Destroy(this.gameObject);
            }
            animator.SetTrigger(animUse);
        }
    }

    public virtual void OnUse() 
    {
        onUsing = false;
    }

}
