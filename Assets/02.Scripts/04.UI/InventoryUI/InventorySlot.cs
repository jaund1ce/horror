using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UHFPS.Runtime.InventoryItem;

public class InventorySlot : MonoBehaviour
{
    public ItemData CurrentItemData = null;
    public Image CurrentItemImage;
    public TextMeshProUGUI CurrentItemAmount;
    private int amount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ResetSlot();
    }

    public void SetNewData(ItemData itemData)
    {
        CurrentItemData = itemData;
        amount = 1;

        ChangeUI();
    }

    public void ChangeSlotAmount()
    {
        amount += 1;
        ChangeUI();
    }

    public void ResetSlot()
    {
        CurrentItemData = null;
        CurrentItemImage.sprite = null;
        CurrentItemImage.color = Color.black;
        CurrentItemAmount.text = "";
        amount = 0;
    }

    public void OnClick()
    {
        ChangeUI();
    }

    public void OnUse()//아이템이 사용될 경우
    {

    }

    public void ChangeUI()
    {
        CurrentItemImage.color = Color.white;
        CurrentItemImage = CurrentItemData.ItemImage;

        if (CurrentItemData.Stackable)
        {
            CurrentItemAmount.text = amount.ToString();
        }
    }
}
