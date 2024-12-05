using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float itemCheckDistance = 5f;
    [SerializeField] private float itemCheckTime = 0.1f;

    private float lastCheckTime;

    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등

    public ItemBase CurrentInteracteItemData;

    public UIManager temUIManager;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
        playerInputs.Enable();
        //playerInputs.Player.Look.performed += _ => getItemData(); //look이 마우스 델타 값을 받기 때문에 X
    }

    private void Update()
    {
        if (Time.time - lastCheckTime < itemCheckTime) return;

        lastCheckTime = Time.time;
        getItemData();   
    }

    private void getItemData()//현재 바라보는 아이템 표시
    {
        Vector3 sceenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(sceenCenter);

        if(Physics.Raycast(ray,out RaycastHit hit, itemCheckDistance))
        {
            if (hit.collider.TryGetComponent<ItemBase>(out ItemBase itemBase))
            {
                if(itemBase.itemSO == CurrentInteracteItemData)
                {
                    return;
                }

                CurrentInteracteItemData = itemBase;
                temUIManager.OpeninteractPanel();//나중에 메니져가 연결되면 연결, so 값을 넘겨줘서 다른 화면 표시 itemBase.itemSO
            }
            else
            {
                CurrentInteracteItemData = null;
                temUIManager.CloseInteractPanel();
            }
        }
    }

    private void handleInteractionInput(InputAction.CallbackContext context)//상호작용시 아이템 회득
    {
        if (CurrentInteracteItemData == null) return;

        CurrentInteracteItemData.OnInteract();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.Interaction.started += handleInteractionInput;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Interaction.started -= handleInteractionInput;
    }
}
