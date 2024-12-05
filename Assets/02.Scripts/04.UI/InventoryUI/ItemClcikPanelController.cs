using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemClcikPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemUseBTNText;
    private ItemSO currentItemSO;

    private void OnEnable()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void ChangeUsePanelText(ItemSO itemSO)
    {
        if (itemSO.itemData == null) return;

        switch (itemSO.itemData.ID)
        {
            case 0:
                itemUseBTNText.text = "Use";
                Debug.Log("1");
                break;
            case 100:
                itemUseBTNText.text = "Equip";
                Debug.Log("2"); 
                break;
            case 1000:
                itemUseBTNText.text = "Consume";
                Debug.Log("3");
                break;
            default:
                itemUseBTNText.text = "";
                Debug.Log("-1");
                break;
        }
    }

    public void OnUseBTNClick()
    {
        if (currentItemSO == null) return;

        //if(currentItemSO.itemData.)
        ChangeUsePanelText(currentItemSO);
    }
    public void OnAddQuickSlotBTNClick()
    {

    }
}
