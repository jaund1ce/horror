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
    public PlayerInputs playerInputs {  get; private set; }//inputsystem generate c# script로 생성된 스크립트
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //미리 정의한 행동들 move, look,... 등
    public Equipment EquipMent { get; private set; }

    [SerializeField] private CinemachineVirtualCamera playercamera;
    [SerializeField] private GameObject Head;
    [SerializeField] float maxRotateY;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public float rotateSencitivity;
    public bool Rotateable = true;
    public bool RunningReady = false;
    public bool isRunning = false;
    public bool Crouching = false;
    private float currentYangle = 0f;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem에 선언했던 Actionmap 중에 하나를 선택
        EquipMent = GetComponent<Equipment>();
    }

    private void OnEnable()
    {
        UnLockRotate();
        playerInputs.Enable();
        InputSubscribe();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        InputUnsubscribe();
    }

    public void InputSubscribe()
    {
        playerActions.Look.started += RotateCamera;
        playerActions.Run.started += ChangeRunState;
        playerActions.Run.canceled += ChangeRunState2;
        playerActions.EquipmentUse.started += EquipMent.OnAttackInput;
    }

    public void InputUnsubscribe()
    {
        playerActions.Look.started -= RotateCamera;
        playerActions.Run.canceled -= ChangeRunState;
        playerActions.Run.canceled -= ChangeRunState2;
        playerActions.EquipmentUse.started -= EquipMent.OnAttackInput;
    }

    private void LateUpdate()
    {
        UpdateCameraData();
    }

    private void UpdateCameraData()//카메라가 찍고 있는 head의 위치를 mesh로 설정해둔 머리를 쫓아가도록 설정
    {
        Mesh mesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(mesh);
        Vector3 headposition = skinnedMeshRenderer.transform.TransformPoint(mesh.vertices[0]);

        Head.transform.position = headposition + new Vector3(0,0,-0.3f);
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
}
