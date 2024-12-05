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
    public PlayerInputs playerInputs {  get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등

    [SerializeField] private CinemachineVirtualCamera playercamera;
    private CinemachinePOV pov;
    private float rotateSencitivity = 0.02f;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
        pov = playercamera.GetCinemachineComponent<CinemachinePOV>();
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

    private void RotateAll(InputAction.CallbackContext context)//cinemachine의 aim방식에 따라서 회전시키는 방법은 다르다.
    {
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            float rotatex = Mathf.Clamp(delta.y, -60f, 60f);
            float rotatey = Mathf.Clamp(delta.x, -90f, 90f);

            transform.rotation *= Quaternion.Euler(0f, rotatey * rotateSencitivity, 0f);

            if (pov == null) return;

            pov.m_VerticalAxis.Value -= rotatex * rotateSencitivity;
            pov.m_VerticalAxis.Value = Mathf.Clamp(pov.m_VerticalAxis.Value, -45f, 45f);//나중에 외부에서 변경 후 제거
        }

    }
}
