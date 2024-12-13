using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemBase : MonoBehaviour
{
    protected bool onUsing;
    protected string animUse = "Use";
    protected ItemSO itemSO;

    protected Animator animator;
    protected Camera camera;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
        itemSO = MainGameManager.Instance.player.CurrentItemSO;
    }

    public virtual void OnUseInput() 
    {
        if (!onUsing)
        {
            onUsing = true;
            animator.SetTrigger(animUse);
            Debug.Log("Item »ç¿ë");
        }
    }

    public virtual void OnUse() 
    {
        onUsing = false;
    } 
}
