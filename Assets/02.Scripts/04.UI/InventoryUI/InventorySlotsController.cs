using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsController : MonoBehaviour
{
    public PlayerInventoryData InventoryData;
    public InventorySlot[] slots;

    private void Awake()
    {
        //InventoryData = Player.playerinventorydata;
        slots = GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        for(int i=0; i< 15; i++)
        {
            if (InventoryData.inventoryDatas[i].ItemData == null) return;

            InventoryData iD = InventoryData.inventoryDatas[i];
            slots[iD.slotIndex].ChangeData(iD);
        }
    }
}
