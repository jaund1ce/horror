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


    //private void Awake()//인벤토리가 처음 생성되면 안보이고 싶기 때문에 setactive를 꺼준다.
    //{
    //    Inventory.InventoryItemInfoPanelelController = this;
    //}

    private InventoryData currentItemData;

    private void OnDisable()//만약을 위한 방어코드
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

    //## 장착버튼 구현하기
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
