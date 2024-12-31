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
    public PlayerInputs PlayerInputs {  get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��
    public Equipment EquipMent { get; private set; }

    [SerializeField] private CinemachineVirtualCamera playercamera;
    [SerializeField] private GameObject head;
    [SerializeField] float maxRotateY;
    public SkinnedMeshRenderer SkinnedMeshRenderer;

    private float rotateSencitivity;
    public bool Rotateable = true;
    public bool RunningReady = false;
    public bool isCrouching = false;
    private float currentYangle = 0f;

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();
        PlayerActions = PlayerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        EquipMent = GetComponent<Equipment>();
    }

    private void OnEnable()
    {
        UnLockRotate();
        PlayerInputs.Enable();
        InputSubscribe();
    }

    private void OnDisable()
    {
        PlayerInputs.Disable();
        InputUnsubscribe();
    }

    public void InputSubscribe()
    {
        PlayerActions.Look.started += RotateCamera;
        PlayerActions.Run.started += ChangeRunState;
        PlayerActions.Run.canceled += ChangeRunState2;
        PlayerActions.Crouch.started += ChangeCrouchState;
        PlayerActions.Crouch.canceled += ChangeCrouchState2;
        PlayerActions.EquipmentUse.started += EquipMent.OnAttackInput;
    }

    public void InputUnsubscribe()
    {
        PlayerActions.Look.started -= RotateCamera;
        PlayerActions.Run.canceled -= ChangeRunState;
        PlayerActions.Run.canceled -= ChangeRunState2;
        PlayerActions.Crouch.started -= ChangeCrouchState;
        PlayerActions.Crouch.canceled -= ChangeCrouchState2;
        PlayerActions.EquipmentUse.started -= EquipMent.OnAttackInput;
    }

    private void LateUpdate()
    {
        UpdateCameraData();
    }

    private void UpdateCameraData()//ī�޶� ��� �ִ� head�� ��ġ�� mesh�� �����ص� �Ӹ��� y���� �Ѿư����� ����
    {
        Mesh mesh = new Mesh();
        SkinnedMeshRenderer.BakeMesh(mesh);
        Vector3 headposition = SkinnedMeshRenderer.transform.TransformPoint(mesh.vertices[0]);

        head.transform.position = new Vector3(head.transform.position.x, headposition.y, head.transform.position.z);
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
                head.transform.Rotate(Vector3.right, -rotatex * rotateSencitivity * Time.deltaTime, Space.Self);
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
}
