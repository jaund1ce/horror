using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemBase : MonoBehaviour
{
    protected bool onUsing;
    protected string animUse = "Use";
    protected ItemData itemData;

    protected Animator animator;
    protected Camera camera;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
        itemData = MainGameManager.Instance.player.CurrentItemData;
    }

    public virtual void OnUseInput() 
    {
        if (!onUsing)
        {
            onUsing = true;
            animator.SetTrigger(animUse);
            Debug.Log("Item ���");
        }
    }

    public virtual void OnUse() 
    {
        onUsing = false;
    } 
}
