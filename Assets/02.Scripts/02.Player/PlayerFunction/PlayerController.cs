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
    public PlayerInputs PlayerInputs {  get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등
    public Equipment EquipMent { get; private set; }

    [SerializeField] private CinemachineVirtualCamera playercamera;
    public CinemachineBasicMultiChannelPerlin VirtualCameraNoise;
    public GameObject Head;
    [SerializeField] float maxRotateY;

    [SerializeField]private float rotateSencitivity;
    public bool Rotateable = true;
    public bool RunningReady = false;
    public bool isCrouching = false;
    public bool isRunning = false;
    private float currentYangle = 0f;
    private bool subscribed = false;

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();
        PlayerActions = PlayerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
        EquipMent = GetComponent<Equipment>();
    }

    private void OnEnable()
    {
        UnLockRotate();
        PlayerInputs.Enable();
        VirtualCameraNoise = playercamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        InputSubscribe();
    }

    private void OnDisable()
    {
        PlayerInputs.Disable();
        InputUnsubscribe();
    }

    public void InputSubscribe()
    {
        if (!subscribed)
        {
            PlayerActions.Look.started += RotateCamera;
            PlayerActions.Run.started += ChangeRunState;
            PlayerActions.Run.canceled += ChangeRunState2;
            PlayerActions.Crouch.started += ChangeCrouchState;
            PlayerActions.Crouch.canceled += ChangeCrouchState2;
            PlayerActions.EquipmentUse.started += EquipMent.OnAttackInput;
            subscribed = true;
        }
    }

    public void InputUnsubscribe()
    {
        if (subscribed)
        {
            PlayerActions.Look.started -= RotateCamera;
            PlayerActions.Run.canceled -= ChangeRunState;
            PlayerActions.Run.canceled -= ChangeRunState2;
            PlayerActions.Crouch.started -= ChangeCrouchState;
            PlayerActions.Crouch.canceled -= ChangeCrouchState2;
            PlayerActions.EquipmentUse.started -= EquipMent.OnAttackInput;
            subscribed = false;
        }
    }

    private void LateUpdate()
    {
        OnUsing();
    }

    private void RotateCamera(InputAction.CallbackContext context)//cinemachine의 aim방식에 따라서 회전시키는 방법은 다르다.
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
    }

    public void UnLockRotate()
    {
        Rotateable = true;
    }

    private void ChangeRunState(InputAction.CallbackContext context)
    {
        RunningReady = true; 
    }
    private void ChangeRunState2(InputAction.CallbackContext context)
    {
        RunningReady = false;
    }

    private void ChangeCrouchState(InputAction.CallbackContext context)
    {
        isCrouching = true;
    }
    private void ChangeCrouchState2(InputAction.CallbackContext context)
    {
        isCrouching = false;
    }

    public void ChangeRotateSencitivity(float amount)
    {
        rotateSencitivity = amount;
    }

    public void OnUsing()
    {
        if (EquipMent.CurEquip == null)
        {
            return;
        }
        bool onusing = EquipMent.CurEquip.OnUsing;
        MainGameManager.Instance.Player.Animator.SetBool("OnUsing", onusing);
    }
}
