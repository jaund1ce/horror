using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs {  get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��
    
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        playerInputs.Enable();
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
