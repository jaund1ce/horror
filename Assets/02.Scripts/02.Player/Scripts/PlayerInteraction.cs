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

    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��

    public IInteractable CurrentInteracteable;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        playerInputs.Enable();
        //playerInputs.Player.Look.performed += _ => getItemData(); //look�� ���콺 ��Ÿ ���� �ޱ� ������ X
    }

    private void Update()
    {
        if (Time.time - lastCheckTime < itemCheckTime) return;

        lastCheckTime = Time.time;
        getItemData();   
    }

    private void getItemData()//���� �ٶ󺸴� ������ ǥ��
    {
        Vector3 sceenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(sceenCenter);

        if(Physics.Raycast(ray,out RaycastHit hit, itemCheckDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable iteractable))
            {
                if(iteractable == CurrentInteracteable)
                {
                    return;
                }

                CurrentInteracteable = iteractable;

                if(CurrentInteracteable is ItemBase)
                {
                    //UIManger.OpenInteractPanel((ItemBase)CurrentInteracteable);
                }
                //else if(CurrentInteracteable is )
            }
            else
            {
                CurrentInteracteable = null;
                //temUIManager.CloseInteractPanel();
            }
        }
    }

    private void handleInteractionInput(InputAction.CallbackContext context)//��ȣ�ۿ�� ������ ȸ��
    {
        if (CurrentInteracteable == null) return;

        CurrentInteracteable.OnInteract();
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
