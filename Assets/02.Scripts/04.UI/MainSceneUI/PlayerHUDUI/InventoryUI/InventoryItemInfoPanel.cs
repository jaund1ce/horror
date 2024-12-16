using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UHFPS.Runtime.InventoryItem;

public class InventoryItemInfoPanelelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemUseBTNText;
    [SerializeField] private InventoryController Inventory;


    private void Awake()
    {
        Inventory.InventoryItemInfoPanelelController = this;
    }

    private InventoryData currentItemData;

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void ChangePanelText(InventoryData itemData)
    {
        if (itemData.ItemData == null) return;

        gameObject.SetActive(true);
        currentItemData = itemData;

        itemNameText.text = currentItemData.ItemData.itemSO.ItemNameKor;
        itemDescriptionText.text = currentItemData.ItemData.itemSO.ItemDescription;

        switch (currentItemData.ItemData.itemSO.ItemType)
        {
            case ItemType.EquipItem:
                itemUseBTNText.text = "Equip";
                Debug.Log("1");
                break;
            case ItemType.CcItem:
                itemUseBTNText.text = "Use";
                Debug.Log("2"); 
                break;
            case ItemType.CnsItem:
                itemUseBTNText.text = "Consume";
                Debug.Log("3");
                break;
            default:
                itemUseBTNText.text = "";
                Debug.Log("index Error");
                break;
        }
    }

    //## ������ư �����ϱ�
    public void OnEquipBTNClick()
    {
        if (currentItemData == null) return;

         MainGameManager.Instance.player.CurrentItemData = Inventory.CurrentInventoryData.ItemData;
         
            /*if (currentItemData.amount <= 0)
            {
                currentItemData.ResetData();
                return;
            }*/
        
    }
    public void OnAddQuickSlotBTNClick()
    {
        MainGameManager.Instance.Player.isChangingQuickSlot = true;
        Debug.Log("QuickslotAdd Ready");
    }
}
