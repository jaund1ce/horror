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
    [SerializeField] private InventoryController inventoryLH;
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
        if (CurrentData.quickslotIndex != quickIndex) { ResetSlot(); return; }

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
                if (inventoryLH.CurrentInventoryData.quickslotIndex == -1)
                {
                    CurrentData = inventoryLH.CurrentInventoryData;
                    CurrentData.quickslotIndex = quickIndex;
                }
                else//이미 다른 슬롯에 있는데 빈 공간을 고른 경우
                {
                    inventoryLH.CurrentInventoryData.quickslotIndex = -1;
                    CurrentData = inventoryLH.CurrentInventoryData;
                    CurrentData.quickslotIndex = quickIndex;
                }
            }
            else
            {//index 바꾸기
                CurrentData.quickslotIndex = inventoryLH.CurrentInventoryData.quickslotIndex;
                inventoryLH.CurrentInventoryData.quickslotIndex = quickIndex;

                CurrentData = inventoryLH.CurrentInventoryData;
            }
            MainGameManager.Instance.Player.isChangingQuickSlot = false;
            inventoryLH.QuickslotController.SetQuickSlotUI();
        }
        else
        {
            Debug.Log("Cant add now");
        }
    }

    public void OnUse()//퀵슬롯 버튼 사용 시
    {
        if (CurrentData.ItemData.itemSO.ItemType == ItemType.CnsItem)
        {
            CurrentData.Use(1); // ##TODO : int int 값을 Quantity 로


/*            if (CurrentData.amount <= 0)
            {
                CurrentData.ResetData();
                ResetSlot();
                return;
            }*/
        }
        else if (CurrentData.ItemData.itemSO.ItemType == ItemType.EquipItem)
        {
            //Player.EquipItemdata = CurrentItemData.ItemData;
        }
    }   

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.itemSO.ItemImage;
        CurrentItemAmount.text = (CurrentData.amount).ToString();
    }
}
