using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public InventoryLH inventory;
    private PlayerInventoryData InventoryData;
    public QuickSlot[] quickSlots;

    private void Awake()
    {
        inventory.QuickslotController = this;
        InventoryData = MainGameManager.Instance.Player.playerInventoryData;
        quickSlots = GetComponentsInChildren<QuickSlot>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < 15; i++)
        {
            int qIndex = InventoryData.inventoryDatas[i].quickslotIndex;
            if (qIndex < 0) return;

            quickSlots[qIndex].Add(InventoryData.inventoryDatas[i]);
        }
    }
}
