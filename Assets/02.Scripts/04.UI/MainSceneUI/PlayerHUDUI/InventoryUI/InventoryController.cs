using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour //Player ó�� �κ��丮�� ������ ������ ����
{
    public InventorySlotsController InventoryslotsController;
    public QuickSlotController QuickslotController;
    public InventoryItemInfoPanelelController InventoryItemInfoPanelelController;
    //[SerializeField] private UsePanel usePanel;

    public InventoryData CurrentInventoryData;

    public void ChangeData(InventoryData inventoryData)
    {
        if (CurrentInventoryData == inventoryData) return;

        CurrentInventoryData = inventoryData;
        InventoryItemInfoPanelelController.gameObject.SetActive(true);
        InventoryItemInfoPanelelController.ChangePanelText(CurrentInventoryData);
    }
}
