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
        if (CurrentData.QuickslotIndex != quickIndex) { ResetSlot(); return; }

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
            SoundManger.Instance.MakeEnviornmentSound("Click2");
            if (CurrentData == null) 
            {
                if (inventoryLH.CurrentInventoryData.QuickslotIndex == -1)
                {
                    CurrentData = inventoryLH.CurrentInventoryData;
                    CurrentData.QuickslotIndex = quickIndex;
                }
                else//�̹� �ٸ� ���Կ� �ִµ� �� ������ �� ���
                {
                    inventoryLH.CurrentInventoryData.QuickslotIndex = -1;
                    CurrentData = inventoryLH.CurrentInventoryData;
                    CurrentData.QuickslotIndex = quickIndex;
                }
            }
            else
            {//index �ٲٱ�
                CurrentData.QuickslotIndex = inventoryLH.CurrentInventoryData.QuickslotIndex;
                inventoryLH.CurrentInventoryData.QuickslotIndex = quickIndex;

                CurrentData = inventoryLH.CurrentInventoryData;
            }
            MainGameManager.Instance.Player.isChangingQuickSlot = false;
            inventoryLH.QuickslotController.SetQuickSlotUI();
        }
        else
        {
            SoundManger.Instance.MakeEnviornmentSound("InventoryError");
            Debug.Log("Cant add now");
        }
    }

    public void OnUse()//������ ������ ��� ��
    {
        if (CurrentData.ItemData.itemSO.ItemType == ItemType.CnsItem)
        {
            CurrentData.Use(1); // ##TODO : int int ���� Quantity ��


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
        CurrentItemAmount.text = (CurrentData.Amount).ToString();
    }
}
