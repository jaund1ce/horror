using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsController : MonoBehaviour
{
    [SerializeField]private InventoryLH inventoryLH;
    public InventorySlot[] slots;

    private void Awake()
    {
        //InventoryData = GameManger.Instance.Player.playerinventorydata;
        inventoryLH.InventoryslotsController = this;
        slots = GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        PlayerInventoryData playerInventoryData = null;// GameManger.instance.Player.playerinventorydata;

        for(int i=0; i< 15; i++)
        {
            if (playerInventoryData.inventoryDatas[i].ItemData == null) return;

            InventoryData ID = playerInventoryData.inventoryDatas[i];
            slots[ID.slotIndex].ChangeData(ID);
        }
    }
}
