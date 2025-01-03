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
    private Player player;

    private InventoryData currentItemData;

    private void OnEnable()
    {
        player = MainGameManager.Instance.Player;
    }

    private void OnDisable()//만약을 위한 방어코드
    {
        gameObject.SetActive(false);
    }

    public void ChangePanelText(InventoryData itemData)
    {
        if(itemData.ItemData == null) return;

        gameObject.SetActive(true);
        currentItemData = itemData;

        itemNameText.text = currentItemData.ItemData.itemSO.ItemNameKor;
        itemDescriptionText.text = currentItemData.ItemData.itemSO.ItemDescription;

        if(player.CurrentEquipItem == currentItemData)
        {
            itemUseBTNText.text = "UnEquip";
            return;
        }

        switch (currentItemData.ItemData.itemSO.ItemType)
        {
            case ItemType.EquipItem:
                itemUseBTNText.text = "Equip";
                break;
            case ItemType.CcItem:
                itemUseBTNText.text = "Use";
                break;
            case ItemType.CnsItem:
                itemUseBTNText.text = "Consume";
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
        if (currentItemData == player.CurrentEquipItem)
        {
            SoundManger.Instance.MakeEnviormentSound("Click3");
            player.CurrentEquipItem = null;
            return;
        }

        SoundManger.Instance.MakeEnviormentSound("Click3");
        player.CurrentEquipItem = Inventory.CurrentInventoryData;
        player.Input.EquipMent.EquipNew(player.CurrentEquipItem);        
    }
    public void OnAddQuickSlotBTNClick()
    {
        SoundManger.Instance.MakeEnviormentSound("Click3");
        player.isChangingQuickSlot = true;
    }
}
