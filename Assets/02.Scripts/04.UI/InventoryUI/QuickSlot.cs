using System.Collections;
using System.Collections.Generic;
using TMPro;
using UHFPS.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private InventoryData CurrentData = null;
    [SerializeField] private Image CurrentItemImage;
    [SerializeField] private TextMeshProUGUI CurrentItemAmount;
    [SerializeField] private InventoryLH inventoryLH;
    public int quickIndex;

    public void Add(InventoryData inventoryData)
    {
        CurrentData = inventoryData;
        ChangeData();
    }

    public void ChangeData()
    {
        if (CurrentData == null) { ResetSlot(); return; }
        if (CurrentData.ItemData == null) { ResetSlot(); return; }

        ChangeUI();
    }

    private void ResetSlot()
    {
        CurrentItemImage.sprite = null;
        CurrentItemAmount.text = "";
    }

    public void OnClick()
    {
        Debug.Log("Adding");
        if (MainGameManager.Instance.Player.isChangingQuickSlot)
        {
            if (CurrentData == null) 
            {
                CurrentData = inventoryLH.CurrentInventoryData;
                CurrentData.quickslotIndex = quickIndex;
            }
            else
            {//index 바꾸기
                CurrentData.quickslotIndex = inventoryLH.CurrentInventoryData.quickslotIndex;
                inventoryLH.CurrentInventoryData.quickslotIndex = quickIndex;

                CurrentData = inventoryLH.CurrentInventoryData;
            }
            MainGameManager.Instance.Player.isChangingQuickSlot = false;
            ChangeUI();
        }
        else
        {
            Debug.Log("Cant add now");
        }
    }

    public void OnUse()//퀵슬롯 버튼 사용 시
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
