using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemClcikPanInventoryItemInfoPanelelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemUseBTNText;
    [SerializeField] private InventoryLH InventoryLH;

    private void Awake()
    {
        InventoryLH.ItemClcikPanInventoryItemInfoPanelelController = this;
    }

    private ItemSO currentItemSO;

    private void OnEnable()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void ChangePanelText(ItemSO itemSO)
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

        //GameManager.Instance.Player.playerinventorydata.selectinventorydata = CurrentData;
    }
    public void OnAddQuickSlotBTNClick()
    {
        //InventoryLH.quickslotController.Add();
    }
}
