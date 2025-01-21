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
    public bool isInventoryOpen = false;

    private void OnEnable()
    {
        SoundManger.Instance.MakeEnviormentSound("InventoryOpen", 1f);
    }

    private void OnDisable()
    {
        SoundManger.Instance.MakeEnviormentSound("InventoryClose", 1f);
    }

    public void ChangeData(InventoryData inventoryData)
    {
        if (inventoryData == null)
        {
            CurrentInventoryData = null;
            InventoryItemInfoPanelelController.gameObject.SetActive(false);
            return;
        }
        if (CurrentInventoryData == inventoryData) return;

        CurrentInventoryData = inventoryData;
        InventoryItemInfoPanelelController.gameObject.SetActive(true);
        InventoryItemInfoPanelelController.ChangePanelText(CurrentInventoryData);
    }
}
