using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
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
    [SerializeField] private GameObject Head;
    [SerializeField] float maxRotateY;
    public float rotateSencitivity;
    public bool Rotateable = true;
    public bool RunningReady = false;
    public bool isRunning = false;
    private float currentYangle = 0f;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
    }

    private void OnEnable()
    {
        UnLockRotate();
        playerInputs.Enable();
        playerActions.Look.started += RotateCamera;
        playerActions.Run.started += ChangeRunState;
        playerActions.Run.canceled += ChangeRunState2;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Look.started -= RotateCamera;
        playerActions.Run.canceled -= ChangeRunState;
        playerActions.Run.canceled -= ChangeRunState2;
    }



    private void RotateCamera(InputAction.CallbackContext context)//cinemachine�� aim��Ŀ� ���� ȸ����Ű�� ����� �ٸ���.
    {
        if (!Rotateable) return;
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta != Vector2.zero)
        {
            float rotatex = Mathf.Clamp(delta.y, -maxRotateY, maxRotateY);
            float rotatey = Mathf.Clamp(delta.x, -60f, 60f);

            transform.Rotate(Vector3.up, rotatey * rotateSencitivity * Time.deltaTime);

            if (currentYangle - rotatex * rotateSencitivity * Time.deltaTime < maxRotateY && currentYangle - rotatex * rotateSencitivity * Time.deltaTime > -maxRotateY)
            {
                currentYangle -= rotatex * rotateSencitivity * Time.deltaTime;
                Head.transform.Rotate(Vector3.right, -rotatex * rotateSencitivity * Time.deltaTime, Space.Self);
            }
        }
    }

    public void LockRotate() 
    {
        Rotateable = false;
        //pov.m_HorizontalAxis.m_MaxSpeed = 0;
        //pov.m_VerticalAxis.m_MaxSpeed = 0;
        //pov.m_HorizontalAxis.m_InputAxisName = "";
        //pov.m_VerticalAxis.m_InputAxisName = "";
    }

    public void UnLockRotate()
    {
        Rotateable = true;
        //pov.m_HorizontalAxis.m_MaxSpeed = rotateXSencitivity;
        //pov.m_VerticalAxis.m_MaxSpeed = rotateXSencitivity;
        //pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        //pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
    }

    private void ChangeRunState(InputAction.CallbackContext context)
    {
        RunningReady = true; 
    }
    private void ChangeRunState2(InputAction.CallbackContext context)
    {
        RunningReady = false;
    }
}
