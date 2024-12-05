using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs { get; private set; } // inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; } // �̸� ������ �ൿ�� move, look,... ��

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private float rotateSensitivity = 0.02f;
    [SerializeField] private float verticalClampMin = -45f;
    [SerializeField] private float verticalClampMax = 45f;

    private CinemachinePOV pov;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player; // inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.Look.performed += RotateAll; // performed�� ����
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Look.performed -= RotateAll;
    }

    private void RotateAll(InputAction.CallbackContext context) // cinemachine�� aim��Ŀ� ���� ȸ����Ű�� ����� �ٸ���.
    {
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            float rotateX = delta.y * rotateSensitivity * Time.deltaTime;
            float rotateY = delta.x * rotateSensitivity * Time.deltaTime;

            transform.rotation *= Quaternion.Euler(0f, rotateY, 0f);

            if (pov != null)
            {
                pov.m_VerticalAxis.Value -= rotateX;
                pov.m_VerticalAxis.Value = Mathf.Clamp(pov.m_VerticalAxis.Value, verticalClampMin, verticalClampMax);
            }
        }
    }
}