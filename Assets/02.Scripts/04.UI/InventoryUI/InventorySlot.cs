using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UHFPS.Runtime.InventoryItem;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]private InventoryData CurrentData = null;
    [SerializeField]private Image CurrentItemImage;
    [SerializeField]private TextMeshProUGUI CurrentItemAmount;

    // Start is called before the first frame update
    void OnEnable()
    {
        ChangeData(CurrentData);
    }

    public void ChangeData(InventoryData itemData)
    {
        if (itemData == null) { ResetSlot(); return; }

        CurrentData = itemData;
        ChangeUI();
    }

    private void ResetSlot()
    {
        CurrentData = null;
        CurrentItemImage.sprite = null;
        CurrentItemImage.color = Color.black;
        CurrentItemAmount.text = "";
    }

    public void OnClick()
    {
        Debug.Log("Click!");
    }

    public void OnUse()//아이템이 사용될 경우
    {

    }

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.ItemImage;
        CurrentItemAmount.text = (CurrentData.amount).ToString();
    }
}
