using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQuickSlotUsing : MonoBehaviour
{
    private Player player;
    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.QuickSlots.started += UseQuick;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.QuickSlots.started -= UseQuick;
    }

    private void UseQuick(InputAction.CallbackContext context)
    {
        int index = int.Parse(context.control.displayName);

        CheckQuickSlot(index);
    }

    private void CheckQuickSlot(int i)
    {
        foreach(InventoryData inventoryData in player.playerInventoryData.inventoryDatas)
        {
            if(inventoryData == null) continue;
            if(inventoryData.quickslotIndex == i-1)
            {
                EquipQuick(inventoryData);
                return;
            }
        }
    }

    private void EquipQuick(InventoryData inventoryData)
    {
        MainGameManager.Instance.Player.CurrentEquipItem = inventoryData;
        MainGameManager.Instance.Player.Input.EquipMent.EquipNew(MainGameManager.Instance.Player.CurrentEquipItem);
    }
}
