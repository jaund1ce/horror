using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float itemCheckDistance = 100f;

    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��

    public ItemSO CurrentInteracteItemData;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        playerInputs.Enable();

        playerActions.Interaction.performed += _ => handleInteractionInput();
        //playerInputs.Player.Look.performed += _ => getItemData(); //look�� ���콺 ��Ÿ ���� �ޱ� ������ X
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
                    //GameManger.instance.UIManger.OpenInteractPanel//���߿� �޴����� ����Ǹ� ����
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
