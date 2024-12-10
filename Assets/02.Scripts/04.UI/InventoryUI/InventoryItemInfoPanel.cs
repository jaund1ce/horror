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
    [SerializeField] private InventoryLH InventoryLH;

    private void Awake()
    {
        InventoryLH.InventoryItemInfoPanelelController = this;
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

        itemNameText.text = currentItemData.ItemData.ItemNameEng;
        //itemDescriptionText.text = currentItemData.ItemData.ItemDescription;

        switch (currentItemData.ItemData.ItemType)
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

    public void OnUseBTNClick()
    {
        if (currentItemData == null) return;

        if (currentItemData.ItemData.ItemType == ItemType.EquipItem)
        {
        }
        else
        {
            currentItemData.amount -= 1;

            if (currentItemData.amount <= 0)
            {
                currentItemData.ResetData();
                return;
            }
        }
    }
    public void OnAddQuickSlotBTNClick()
    {
        MainGameManager.Instance.Player.isChangingQuickSlot = true;
        Debug.Log("QuickslotAdd Ready");
    }
}
