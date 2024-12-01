using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs {  get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��

    [SerializeField] private CinemachineVirtualCamera playercamera;
    private CinemachineComposer composer;
    private float rotateSencitivity = 0.02f;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        composer = playercamera.GetCinemachineComponent<CinemachineComposer>();
        playerInputs.Enable();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.Look.started += RotateAll;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Look.started -= RotateAll;
    }

    private void RotateAll(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            Transform playerTransform = transform;

            float rotatex = Mathf.Clamp(delta.y, -90f, 90f);
            float rotatey = Mathf.Clamp(delta.x, -90f, 90f);

            playerTransform.rotation *= Quaternion.Euler(0f, rotatey * rotateSencitivity, 0f);
            //composer.m_TrackedObjectOffset.y = rotatex;
        }

    }
}
