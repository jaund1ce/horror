using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private InventoryData CurrentData = null;
    [SerializeField] private Image CurrentItemImage;
    [SerializeField] private TextMeshProUGUI CurrentItemAmount;
    public int quickIndex;

    // Start is called before the first frame update
    void OnEnable()
    {
        ChangeData(CurrentData);
    }

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
        if (true) // GameMAnge.instance.Player.isChangingQuickSlot;
        {
            if (CurrentData == null) 
            {
                //CurrentData = UIManger.inventoryLH.currentSelectData;
                CurrentData.quickslotIndex = quickIndex;
            }
            else
            {
                quickIndex = CurrentData.quickslotIndex;
                //CurrentData.quickslotIndex = UIManger.inventoryLH.currentSelectData.quickslotIndex;
                //CurrentData = UIManger.inventoryLH.currentSelectData;
                CurrentData.quickslotIndex = quickIndex;
            }
            ChangeUI();
        }
    }

    public void OnUse()//Äü½½·Ô ¹öÆ° »ç¿ë ½Ã
    {
        if (CurrentData.ItemData.ItemType == ItemType.CnsItem)
        {
            CurrentData.amount -= 1;

            if (CurrentData.amount <= 0)
            {
                CurrentData.ResetData();
                ResetSlot();
                return;
            }
        }
        else if (CurrentData.ItemData.ItemType == ItemType.EquipItem)
        {
            //Player.EquipItemdata = CurrentItemData.ItemData;
        }
    }   

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.ItemImage;
        CurrentItemAmount.text = (CurrentData.amount).ToString();
    }
}
