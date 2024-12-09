using System.Collections;
using System.Collections.Generic;
using TMPro;
using UHFPS.Runtime;
using UnityEngine;
using UnityEngine.UI;

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
        if (itemData.ItemData == null) { ResetSlot(); return; }

        CurrentData = itemData;
        ChangeUI();
    }

    private void ResetSlot()
    {
        if (CurrentData != null)
        {
            CurrentData.ItemData = null;
            CurrentData.amount = -1;
        }

        CurrentItemImage.sprite = null;
        CurrentItemImage.color = Color.black;
        CurrentItemAmount.text = "";
    }

    public void OnClick()
    {
        //GameManager.Instance.Player.changeSelectItem?.invoke(CurrentData);
        Debug.Log("Click!");
    }

    public void OnUse()//아이템이 사용될 경우
    {
        CurrentData.amount -= 1;

        if (CurrentData.amount <= 0)
        {
            ResetSlot();
            return;
        }
        ChangeUI();
    }

    public void ChangeUI()
    {
        CurrentItemImage.sprite = CurrentData.ItemData.ItemImage;
        CurrentItemAmount.text = (CurrentData.amount).ToString();
    }
}
