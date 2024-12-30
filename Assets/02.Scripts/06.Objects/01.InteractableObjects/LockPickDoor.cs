using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockPickDoor : PuzzleBase
{
    [Header("Door Settings")]
    private Transform hinge; 
    private float openAngle = -90f; 
    private float closeAngle = 0f; 
    private float openSpeed = 5f; 
    private bool isOpened = false;
    [HideInInspector]public bool IsLocked = true;

    [Header("LcokPick Settings")]
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private Transform pin;
    [field: SerializeField] private Transform keyhole;
    [field: SerializeField] private Transform lockPickPoint; //락픽이 꽂혀져보이는 정확한 지점
    [field: SerializeField] private SoundClip Unlock;
    [field: SerializeField] private float UnlockAngle; //## 추후에 랜덤으로 넣거나 아니면 SO 로 넣어줄 예정
    [field: SerializeField] private GameObject lockPickUI;
    private Vector3 pinCenterPoint = Vector3.forward;
    private Vector3 keyholeCenterPoint  = Vector3.forward;
    private float keyHoleTargetAngle;
    private float keyholeUnlockAngle = -90f;
    private float keyholeRotateSpeed = 2f;
    private float currentKeyholeAngle;
    private float keyholeUnlockDistance;

    [Header("Pin Settings")]
    [field: SerializeField] private AudioClip pinBreak;
    private float pinRotateSpeed = 20f;
    private float pinLifeTime = 2f;
    private float currentPinLifeTime;
    private float pinResetTime = 1f;
    private float pinShakeAmount = 3f;
    private float currentPinAngle;
    private float pinUnlockDistance;
    private float marginOfErrorAngle = 2f; // 정답 각도와의 오차 범위

    private bool canUsePin = true;
    private bool tryUnlock;


    private void OnEnable()
    {
        hinge = this.transform;
        currentPinLifeTime = pinLifeTime;
        lockPickUI.SetActive(false);
    }

    private void Update()
    {
        if (!isUsingPuzzle || !tryUnlock) return;

        float pinAngleDiff = Mathf.Abs(UnlockAngle - currentPinAngle); //잠금성공 각도와 현재 핀 각도 차이
        Debug.Log(pinAngleDiff);
        float pinNormalized = 0f;
        float pinShake = 0f;

        if (lockPickPoint != null && !canUsePin)// ## 핀 갯수 체크도 같이 넣어야함
        {
            Invoke("InitializePinPosition", 1f);
        }

        if (IsLocked && canUsePin)
        {
            bool damageToPin = true;

            float randomShake = UnityEngine.Random.insideUnitCircle.x;
            pinShake = UnityEngine.Random.Range(-randomShake, randomShake) * pinShakeAmount;

            if (pinAngleDiff <= marginOfErrorAngle)
            {
                pinNormalized = 1 - (pinAngleDiff / marginOfErrorAngle);
                pinNormalized = (float)Math.Round(pinNormalized, 2);
                float targetDiff = Mathf.Abs(keyHoleTargetAngle - currentKeyholeAngle);
                float targetNormalized = targetDiff / marginOfErrorAngle;

                Debug.Log($"pinNormalized : {pinNormalized}  대상 : {1-pinUnlockDistance}");
                if (pinNormalized >= (1 - pinUnlockDistance))
                {
                    pinNormalized = 1;
                    damageToPin = false;
                    pinShake = 0;

                    if (targetNormalized <= keyholeUnlockDistance)
                    {
                        //잠금 해제 성공 코루틴 작성
                        IsLocked = false;
                    }
                }
            }


            if (damageToPin)
            {
                if (currentPinLifeTime > 0)
                {
                    currentPinLifeTime -= Time.deltaTime;
                }
                else
                {
                    //인벤토리의 수량감소 작성
                    pin.gameObject.SetActive(false);
                    currentPinLifeTime = pinLifeTime;
                    audioSource.PlayOneShot(pinBreak);
                    canUsePin = false;
                    currentPinAngle = 0;
                }
            }

            pin.localRotation = Quaternion.Euler(0f, 0f, currentPinAngle + pinShake);

        }

        if (!IsLocked)
        {
            keyHoleTargetAngle = keyholeUnlockAngle;
            pinNormalized = 1f;
        }

        keyHoleTargetAngle *= pinNormalized;
        currentKeyholeAngle = Mathf.MoveTowardsAngle(currentKeyholeAngle, keyHoleTargetAngle, Time.deltaTime * keyholeRotateSpeed * 100f);
        currentKeyholeAngle = Mathf.Clamp(currentKeyholeAngle, 0, -90);
        //keyhole.localRotation = Quaternion.AngleAxis(currentKeyholeAngle, keyholeCenterPoint);
    }


    private void ForceToPin(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {
            //누르면 왼쪽으로 90도 안누르면 0도
            keyHoleTargetAngle = keyholeUnlockAngle;
            tryUnlock = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            keyHoleTargetAngle = 0f;
            tryUnlock = false;
        }
    }

    private void RotatePin(InputAction.CallbackContext context)
    {
        if (tryUnlock) return;
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        float rotationDelta = mouseDelta.x * pinRotateSpeed * Time.deltaTime;
        currentPinAngle += rotationDelta;
        currentPinAngle = Mathf.Clamp(currentPinAngle, -90f, 90f);
        pin.localRotation = Quaternion.Euler(0f, 0f, currentPinAngle);
    }

    private void InitializePinPosition() 
    {
        //## 핀 갯수가 모자라면 UI 감추기
        pin.gameObject.SetActive(true);
        pin.position = lockPickPoint.position;
        canUsePin = true;
    }

    public override void OnInteract()
    {
        if (!isAccess)
        {
            base.OnInteract();
        }
        else 
        {
            ToggleDoor();
        } 
    }

    protected override void EnterPuzzleView()
    {
        if (PuzzleCamera != null)
        {
            PuzzleCamera.Priority = 11; // 카메라 활성화
        }
        isUsingPuzzle = true;
        Invoke("PopupPuzzleUI", 2f);
        MainGameManager.Instance.Player.Input.playerActions.Look.started += RotatePin;
        MainGameManager.Instance.Player.Input.playerActions.EquipmentUse.performed += ForceToPin;
        MainGameManager.Instance.Player.Input.playerActions.EquipmentUse.canceled += ForceToPin;
    }

    protected override void ExitPuzzleView()
    {
        if (PuzzleCamera != null)
        {
            PuzzleCamera.Priority = 9; // 카메라 비활성화
        }
        isUsingPuzzle = false;
        PopupPuzzleUI();
        MainGameManager.Instance.Player.Input.playerActions.Look.started -= RotatePin;
        MainGameManager.Instance.Player.Input.playerActions.EquipmentUse.performed -= ForceToPin;
        MainGameManager.Instance.Player.Input.playerActions.EquipmentUse.canceled -= ForceToPin;
    }


    private void PopupPuzzleUI() 
    {
        if (isUsingPuzzle)
        {
            lockPickUI.SetActive(true);
        }
        else 
        {
            lockPickUI.SetActive(false);
        }
    }

    

    private void ToggleDoor()
    {
        //잠긴문 Interact 시 Sound 추가
        if (IsLocked) return;

        isOpened = !isOpened;

        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpened ? openAngle : closeAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        Debug.Log("문열림 코루틴 시작");
        float currentAngle = hinge.localEulerAngles.y;
        if (currentAngle > 180) currentAngle -= 360;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * openSpeed);
            hinge.localEulerAngles = new Vector3(0, currentAngle, 0);
            yield return null;
        }

        hinge.localEulerAngles = new Vector3(0, targetAngle, 0);
    }

    public override string GetInteractPrompt()
    {
        if (IsLocked) return "Locked";
        return isOpened ? "Close" : "Open";
    }
}
