using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs {  get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등

    [SerializeField] private CinemachineVirtualCamera playercamera;
    private CinemachineComposer composer;
    private float rotateSencitivity = 0.02f;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
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
