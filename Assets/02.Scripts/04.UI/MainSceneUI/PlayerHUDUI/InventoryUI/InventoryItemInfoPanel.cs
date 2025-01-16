using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfoPanelelController : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemUseBTNText;
    [SerializeField] private GameObject addToQuickslotBTN;
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

        itemImage.sprite = currentItemData.ItemData.itemSO.ItemImage;
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
            case ItemType.CnsItem:
                itemUseBTNText.text = "Equip";
                break;
            case ItemType.CcItem:
                itemUseBTNText.text = "";
                break;
            default:
                itemUseBTNText.text = "";
                Debug.Log("index Error");
                break;
        }
        if(itemUseBTNText.text == "")
        {
            itemUseBTNText.transform.parent.gameObject.SetActive(false);
            addToQuickslotBTN.SetActive(false);
        }
        else
        {
            itemUseBTNText.transform.parent.gameObject.SetActive(true);
            addToQuickslotBTN.SetActive(true);
        }
    }

    //## 장착버튼 구현하기
    public void OnEquipBTNClick()
    {
        if (currentItemData == null) return;
        if (currentItemData == player.CurrentEquipItem)
        {
            SoundManger.Instance.MakeEnviornmentSound("Click3");
            player.UnEquipCurrentItem();
            ChangePanelText(currentItemData);
            return;
        }

        SoundManger.Instance.MakeEnviornmentSound("Click3");
        player.CurrentEquipItem = Inventory.CurrentInventoryData;
        player.Input.EquipMent.EquipNew(player.CurrentEquipItem);
        player.ChangeEquip();
        ChangePanelText(currentItemData);
    }
    public void OnAddQuickSlotBTNClick()
    {
        SoundManger.Instance.MakeEnviornmentSound("Click3");
        player.isChangingQuickSlot = true;
    }
}
