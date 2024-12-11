using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsController : MonoBehaviour
{
    [SerializeField]private InventoryController inventoryController;
    PlayerInventoryData playerInventoryData;
    public InventorySlot[] slots;

    private void Awake()
    {
        playerInventoryData = MainGameManager.Instance.Player.playerInventoryData;
        inventoryController.InventoryslotsController = this;
        slots = GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        for(int i=0; i< 15; i++)
        {
            if (playerInventoryData.inventoryDatas[i].ItemData == null) return;

            InventoryData ID = playerInventoryData.inventoryDatas[i];
            slots[ID.slotIndex].ChangeData(ID);
        }
    }
}
