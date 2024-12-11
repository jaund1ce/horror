using System.Collections;
using System.Collections.Generic;
using TMPro;
using UHFPS.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]private InventoryData CurrentData = null;
    [SerializeField]private Image CurrentItemImage;
    [SerializeField]private TextMeshProUGUI CurrentItemAmount;
    [SerializeField]private InventoryController InventorySlotsController;

    public void ChangeData(InventoryData itemData)
    {
        if (itemData == null) { ResetSlot(); return; }
        if (itemData.ItemData == null) { ResetSlot(); return; }

        CurrentData = itemData;
        ChangeUI();
    }

    private void ResetSlot()
    {
        CurrentItemImage.sprite = null;
        CurrentItemAmount.text = "";
    }

    public void OnClick()
    {
        Debug.Log("slotclicked!");
        InventorySlotsController.ChangeData(CurrentData);
    }

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.ItemImage;
        CurrentItemAmount.text = (CurrentData.amount).ToString();
    }
}
