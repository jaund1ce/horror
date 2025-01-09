using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemBase : MonoBehaviour
{
    public bool OnUsing { get; protected set; }
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
            if (inventoryData.Use(1) == (int)TryUse.ResetItem) 
            {
                Destroy(this.gameObject);
            }
            Invoke("OnUse", 1f);
            // 애니메이션 추가
        }
    }

    public virtual void OnUse() 
    {
        OnUsing = false;
    }

}
