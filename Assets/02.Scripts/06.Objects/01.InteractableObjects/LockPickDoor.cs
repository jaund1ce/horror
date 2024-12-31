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

    [Header("LcokPick Settings")]
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private Transform pin;
    [field: SerializeField] private Transform keyhole;
    [field: SerializeField] private Transform lockPickPoint; //락픽이 꽂혀져보이는 정확한 지점
    [field: SerializeField] private AudioClip unlock;
    [field: SerializeField] private float unlockAngle; //## 추후에 랜덤으로 넣거나 아니면 SO 로 넣어줄 예정
    [field: SerializeField] private GameObject lockPickUI;
    private float keyHoleTargetAngle;
    private float keyholeUnlockAngle = -90f;
    private float keyholeRotateSpeed = 2f;
    private float currentKeyholeAngle;
    private float keyholeUnlockDistance = 0.1f;

    [Header("Pin Settings")]
    [field: SerializeField] private AudioClip pinBreak;
    private float pinRotateSpeed = 20f;
    private float pinLifeTime = 2f;
    private float currentPinLifeTime;
    private float pinResetTime = 1f;
    private float pinShakeAmount = 3f;
    private float currentPinAngle;
    private float pinUnlockDistance = 0.1f;
    private float marginOfErrorAngle = 20f; // 정답 각도와의 오차 범위

    private InventoryData inventoryPin;
    private bool canUsePin = true;
    private bool tryUnlock;
    private Player player;


    private void OnEnable()
    {
        hinge = this.transform;
        currentPinLifeTime = pinLifeTime;
        lockPickUI.SetActive(false);
        player = MainGameManager.Instance.Player;
    }

    private void Update()
    {
        if (!isUsingPuzzle) return;

        float pinAngleDiff = Mathf.Abs(unlockAngle - currentPinAngle); //잠금성공 각도와 현재 핀 각도 차이
        float pinNormalized = 0f;
        float pinShake = 0f;

        if (lockPickPoint != null)
        {
            Invoke("SetPin", pinResetTime);
            Invoke("InitializePinPosition", pinResetTime);
        }

        pin.position = lockPickPoint.position;

        if (!IsAccess && canUsePin)
        {
            if (tryUnlock)
            {
                bool damageToPin = true;

                float randomShake = UnityEngine.Random.insideUnitCircle.x;
                pinShake = UnityEngine.Random.Range(-randomShake, randomShake) * pinShakeAmount;

                if (pinAngleDiff <= marginOfErrorAngle)
                {
                    keyHoleTargetAngle = -90f;
                    // 각도차이가 5도 일경우 값이 0.75가 나옴
                    // marginOfErrorAngle = 20f; 기준 2도정도까지 오차범위 허용 (** pinUnlockDistance에 따라도 달라짐)
                    // marginOfErrorAngle = 40f; 기준 4도정도까지 오차범위 허용
                    pinNormalized = 1 - (pinAngleDiff / marginOfErrorAngle); 
                    pinNormalized = (float)Math.Round(pinNormalized, 2);
                    float targetDiff = Mathf.Abs(keyHoleTargetAngle - currentKeyholeAngle);
                    float targetNormalized = targetDiff / marginOfErrorAngle;

                    if (pinNormalized >= (1 - pinUnlockDistance))
                    {
                        pinNormalized = 1;
                        damageToPin = false;
                        pinShake = 0;

                        if (targetNormalized <= keyholeUnlockDistance)
                        {
                            player.Interact.HandleInputAndPrompt();
                            ExitPuzzleView();
                            audioSource.PlayOneShot(unlock);
                            IsAccess = true;
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
                        inventoryPin.amount -= 1;
                        pin.gameObject.SetActive(false);
                        currentPinLifeTime = pinLifeTime;
                        audioSource.PlayOneShot(pinBreak);
                        canUsePin = false;
                        currentPinAngle = 0;
                    }
                }
            }
        }
        if (IsAccess)
        {
            keyHoleTargetAngle = keyholeUnlockAngle;
            pinNormalized = 1f;
        }

        keyHoleTargetAngle *= pinNormalized;
        currentKeyholeAngle = Mathf.MoveTowardsAngle(currentKeyholeAngle, keyHoleTargetAngle, Time.deltaTime * keyholeRotateSpeed * 100f);
        currentKeyholeAngle = Mathf.Clamp(currentKeyholeAngle, -90f, 0f);
        keyhole.localRotation = Quaternion.Euler(0f, 0f, currentKeyholeAngle);
        pin.localRotation = Quaternion.Euler(0f, 0f, (currentPinAngle + pinShake) - currentKeyholeAngle / 18);

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
        if (canUsePin)
        {
            pin.gameObject.SetActive(true);
        }
        else 
        {
            pin.gameObject.SetActive(false);
        }
    }

    public override void OnInteract()
    {
        if (!IsAccess)
        {
            base.OnInteract();
        }
        else
        {
            ToggleDoor();
        }
    }

    private void SetPin()
    {
        for (int i= 0; i < player.playerInventoryData.inventoryDatas.Length; i++) 
        {
            if (player.playerInventoryData.inventoryDatas[i]?.ItemData?.itemSO?.ID == 1002)
            {
                inventoryPin = player.playerInventoryData.inventoryDatas[i];
                canUsePin = true;
                return;
            }
        }
        inventoryPin = null;
        canUsePin = false;
    }

    protected override void EnterPuzzleView()
    {
        if (PuzzleCamera != null)
        {
            PuzzleCamera.Priority = 11; // 카메라 활성화
        }
        isUsingPuzzle = true;
        Invoke("PopupPuzzleUI", 2f);
        player.Input.playerActions.Look.started += RotatePin;
        player.Input.playerActions.EquipmentUse.performed += ForceToPin;
        player.Input.playerActions.EquipmentUse.canceled += ForceToPin;
        
    }

    protected override void ExitPuzzleView()
    {
        if (PuzzleCamera != null)
        {
            PuzzleCamera.Priority = 9; // 카메라 비활성화
        }
        isUsingPuzzle = false;
        PopupPuzzleUI();
        player.Input.PlayerActions.Look.started -= RotatePin;
        player.Input.PlayerActions.EquipmentUse.performed -= ForceToPin;
        player.Input.PlayerActions.EquipmentUse.canceled -= ForceToPin;
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
        if (!IsAccess) return;

        isOpened = !isOpened;

        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpened ? openAngle : closeAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
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
        if (!IsAccess) return "Lock..ed..?";
        return isOpened ? "Close" : "Open";
    }
}
