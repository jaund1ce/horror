using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsController : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    PlayerInventoryData playerInventoryData;
    public InventorySlot[] slots;

    private void Awake()
    {
        playerInventoryData = MainGameManager.Instance.Player.playerInventoryData;
        inventoryController.InventoryslotsController = this;
        slots = GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        RefreshSlots(); // 수정됨: OnEnable에서 RefreshSlots 호출
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var inventoryData = playerInventoryData.inventoryDatas[i];

            if (inventoryData.ItemData == null)
            {
                Debug.Log($"Slot {i}: Empty");
                slots[i].ResetSlot();
            }
            else
            {
                Debug.Log($"Slot {i}: {inventoryData.ItemData.itemSO.ItemNameEng}, Amount: {inventoryData.amount}");
                slots[i].ChangeData(inventoryData);
            }
        }
    }

}
