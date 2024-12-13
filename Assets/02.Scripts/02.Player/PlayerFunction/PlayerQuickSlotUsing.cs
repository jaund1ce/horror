using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQuickSlotUsing : MonoBehaviour
{
    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
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
        string index = context.control.displayName;

        switch (index)
        {
            case "1": Debug.Log("1"); break;
            case "2": Debug.Log("2"); break;
            case "3": Debug.Log("3"); break;
            case "4": Debug.Log("4"); break;
            default: Debug.Log("IndexError"); break;
        }
    }
}
