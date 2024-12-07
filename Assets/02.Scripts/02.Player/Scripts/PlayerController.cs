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
    public float rotateXSencitivity;
    public float rotateYSencitivity;
    public bool Rotateable = true;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        pov = playercamera.GetCinemachineComponent<CinemachinePOV>();
        //playerInputs.Enable();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.Look.started += RotateCamera;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Look.started -= RotateCamera;
    }

    private void RotateCamera(InputAction.CallbackContext context)//cinemachine�� aim��Ŀ� ���� ȸ����Ű�� ����� �ٸ���.
    {
        if (!Rotateable) return;
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            float rotatex = Mathf.Clamp(delta.y, -60f, 60f);
            float rotatey = Mathf.Clamp(delta.x, -90f, 90f);

            pov.m_HorizontalAxis.m_MaxSpeed = rotateXSencitivity;

            transform.rotation *= Quaternion.Euler(0f, rotatey * rotateXSencitivity * Time.deltaTime, 0f);

            if (pov == null)
            {
                Debug.LogError("cinemachine pov not found!");
                return;
            }
        }
    }

    public void LockRotate()
    {
        Rotateable = false;
        pov.m_HorizontalAxis.m_MaxSpeed = 0;
        pov.m_VerticalAxis.m_MaxSpeed = 0;
        pov.m_HorizontalAxis.m_InputAxisName = "";
        pov.m_VerticalAxis.m_InputAxisName = "";
    }

    public void UnLockRotate()
    {
        Rotateable = true;
        pov.m_HorizontalAxis.m_MaxSpeed = rotateXSencitivity;
        pov.m_VerticalAxis.m_MaxSpeed = rotateYSencitivity;
        pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
    }
}
