using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float itemCheckDistance = 100f;

    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등

    public ItemSO CurrentInteracteItemData;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
        playerInputs.Enable();

        playerActions.Interaction.performed += _ => handleInteractionInput();
        //playerInputs.Player.Look.performed += _ => getItemData(); //look이 마우스 델타 값을 받기 때문에 X
    }

    private void getItemData()
    {
        Vector3 sceenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(sceenCenter);

        if(Physics.Raycast(ray,out RaycastHit hit, itemCheckDistance))
        {
            if (hit.collider.TryGetComponent<ItemBase>(out ItemBase item))
            {
                if(item.itemSO != CurrentInteracteItemData)
                {
                    CurrentInteracteItemData = item.itemSO;
                    //GameManger.instance.UIManger.OpenInteractPanel//나중에 메니져가 연결되면 연결
                }
            }
        }
    }

    private void handleInteractionInput()
    {
        
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
}
