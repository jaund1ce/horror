using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryItemInfoPanelelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemUseBTNText;
    [SerializeField] private InventoryController Inventory;


    //private void Awake()//�κ��丮�� ó�� �����Ǹ� �Ⱥ��̰� �ͱ� ������ setactive�� ���ش�.
    //{
    //    Inventory.InventoryItemInfoPanelelController = this;
    //}

    private InventoryData currentItemData;

    private void OnDisable()//������ ���� ����ڵ�
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
                break;
            case ItemType.CcItem:
                //itemUseBTNText.text = "Use";
                itemUseBTNText.text = "Equip";
                break;
            case ItemType.CnsItem:
                //itemUseBTNText.text = "Consume";
                itemUseBTNText.text = "Equip";
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

        MainGameManager.Instance.Player.CurrentEquipItem = Inventory.CurrentInventoryData;
        MainGameManager.Instance.Player.Input.EquipMent.EquipNew(MainGameManager.Instance.Player.CurrentEquipItem);        
    }
    public void OnAddQuickSlotBTNClick()
    {
        MainGameManager.Instance.Player.isChangingQuickSlot = true;
        Debug.Log("QuickslotAdd Ready");
    }
}
