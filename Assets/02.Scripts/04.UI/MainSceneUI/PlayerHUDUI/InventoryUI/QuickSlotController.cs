using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public InventoryController inventory;
    private PlayerInventoryData InventoryData;
    public QuickSlot[] quickSlots;

    private void Awake()
    {
        inventory.QuickslotController = this;
        InventoryData = MainGameManager.Instance.Player.PlayerInventoryData;
        quickSlots = GetComponentsInChildren<QuickSlot>();
    }

    private void OnEnable()
    {
        SetQuickSlotUI();
    }

    public void SetQuickSlotUI()
    {
        for (int i = 0; i < 4; i++)
        {
            quickSlots[i].quickIndex = i;
            quickSlots[i].ChangeData();
        }

        for (int i = 0; i < 15; i++)
        {
            int qIndex = InventoryData.inventoryDatas[i].QuickslotIndex;
            if (qIndex < 0) continue;

            quickSlots[qIndex].Add(InventoryData.inventoryDatas[i]);
        }
    }
}
