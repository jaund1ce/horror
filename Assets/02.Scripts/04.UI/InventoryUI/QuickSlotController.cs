using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public InventoryLH inventory;
    private PlayerInventoryData InventoryData;
    public QuickSlot[] quickSlots;

    private void Awake()
    {
        inventory.QuickslotController = this;
        //InventoryData = GameManger.Instance.Player.playerinventorydata;
        quickSlots = GetComponentsInChildren<QuickSlot>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
            if (InventoryData.inventoryDatas[i].ItemData == null) return;

            InventoryData iD = InventoryData.inventoryDatas[i];
            quickSlots[i].ChangeData(iD);
            quickSlots[i].quickIndex = i;
        }
    }
}
