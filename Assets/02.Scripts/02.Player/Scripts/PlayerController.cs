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
    public PlayerInputs playerInputs {  get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��

    [SerializeField] private CinemachineVirtualCamera playercamera;
    private CinemachinePOV pov;
    private float rotateSencitivity = 0.02f;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
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

    private void RotateAll(InputAction.CallbackContext context)//cinemachine�� aim��Ŀ� ���� ȸ����Ű�� ����� �ٸ���.
    {
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            float rotatex = Mathf.Clamp(delta.y, -60f, 60f);
            float rotatey = Mathf.Clamp(delta.x, -90f, 90f);

            transform.rotation *= Quaternion.Euler(0f, rotatey * rotateSencitivity, 0f);

            if (pov == null) return;

            pov.m_VerticalAxis.Value -= rotatex * rotateSencitivity;
            pov.m_VerticalAxis.Value = Mathf.Clamp(pov.m_VerticalAxis.Value, -45f, 45f);//���߿� �ܺο��� ���� �� ����
        }

    }
}
