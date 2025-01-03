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
        playerInventoryData = MainGameManager.Instance.Player.PlayerInventoryData;
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
            Debug.Log(i);
            var inventoryData = playerInventoryData.inventoryDatas[i];

            if (inventoryData.ItemData == null)
            {
                slots[i].ResetSlot();
            }
            else
            {
                slots[i].ChangeData(inventoryData);
            }
        }
    }

}
