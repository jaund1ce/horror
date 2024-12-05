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
    public PlayerInputs playerInputs { get; private set; } // inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; } // 미리 정의한 행동들 move, look,... 등

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private float rotateSensitivity = 0.02f;
    [SerializeField] private float verticalClampMin = -45f;
    [SerializeField] private float verticalClampMax = 45f;

    private CinemachinePOV pov;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player; // inputsystem에 선언했던 Actionmap 중에 하나를 선택
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.Look.performed += RotateAll; // performed로 변경
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Look.performed -= RotateAll;
    }

    private void RotateAll(InputAction.CallbackContext context) // cinemachine의 aim방식에 따라서 회전시키는 방법은 다르다.
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