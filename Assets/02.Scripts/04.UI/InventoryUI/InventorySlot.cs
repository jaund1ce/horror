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
        CurrentItemImage.color = Color.black;
        CurrentItemAmount.text = "";
    }

    public void OnClick()
    {
        //UIManger.Instance.inventoryLH.changeData(CurrentData);
    }

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.ItemImage;
        CurrentItemAmount.text = (CurrentData.amount).ToString();
    }
}
