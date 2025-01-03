using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour //Player 처럼 인벤토리의 정보를 가지고 있음
{
    public InventorySlotsController InventoryslotsController;
    public QuickSlotController QuickslotController;
    public InventoryItemInfoPanelelController InventoryItemInfoPanelelController;
    //[SerializeField] private UsePanel usePanel;

    public InventoryData CurrentInventoryData;
    public bool isInventoryOpen = false;

    private void OnEnable()
    {
        SoundManger.Instance.MakeEnviormentSound("InventoryOpen", 1f);
        isInventoryOpen = true;
    }

    private void OnDisable()
    {
        SoundManger.Instance.MakeEnviormentSound("InventoryClose", 1f);
        isInventoryOpen = false;
    }

    public void ChangeData(InventoryData inventoryData)
    {
        if (CurrentInventoryData == inventoryData) return;

        CurrentInventoryData = inventoryData;
        InventoryItemInfoPanelelController.gameObject.SetActive(true);
        InventoryItemInfoPanelelController.ChangePanelText(CurrentInventoryData);
    }
}
