using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQuickSlotUsing : MonoBehaviour
{
    private Player player;
    private PlayerInputs playerInputs;
    private PlayerInputs.PlayerActions playerActions;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInputs = player.Input.PlayerInputs;
        playerActions = player.Input.PlayerActions;
    }

    private void OnEnable()
    {
        playerActions.QuickSlots.started += UseQuick;
    }

    private void OnDisable()
    {
        playerActions.QuickSlots.started -= UseQuick;
    }

    private void UseQuick(InputAction.CallbackContext context)
    {
        int index = int.Parse(context.control.displayName);

        CheckQuickSlot(index);
    }

    private void CheckQuickSlot(int i)
    {
        foreach(InventoryData inventoryData in player.PlayerInventoryData.inventoryDatas)
        {
            if(inventoryData == null) continue;
            if(inventoryData.QuickslotIndex == i-1)
            {
                EquipQuick(inventoryData);
                return;
            }
        }
    }

    private void EquipQuick(InventoryData inventoryData)
    {
        if (player.CurrentEquipItem != inventoryData) player.EquipItem(inventoryData);
        else player.UnEquipCurrentItem();
    }
}
