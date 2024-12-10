using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLH : MonoBehaviour //Player 처럼 인벤토리의 정보를 가지고 있음
{
    public InventorySlotsController InventoryslotsController;
    public QuickSlotController QuickslotController;
    public InventoryItemInfoPanelelController InventoryItemInfoPanelelController;
    //[SerializeField] private UsePanel usePanel;

    public InventoryData CurrentInventoryData;

    public void ChangeData(InventoryData inventoryData)
    {
        if (CurrentInventoryData == inventoryData) return;

        Debug.Log("UIPAnelOpen");
        CurrentInventoryData = inventoryData;
        InventoryItemInfoPanelelController.ChangePanelText(CurrentInventoryData);
    }
}
