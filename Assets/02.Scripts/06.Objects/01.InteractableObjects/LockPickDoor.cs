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
    [field: SerializeField] private Transform lockPickPoint; //������ ���������̴� ��Ȯ�� ����
    [field: SerializeField] private AudioClip unlock;
    [field: SerializeField] private float unlockAngle; //## ���Ŀ� �������� �ְų� �ƴϸ� SO �� �־��� ����
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
    private float marginOfErrorAngle = 20f; // ���� �������� ���� ����

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

        float pinAngleDiff = Mathf.Abs(unlockAngle - currentPinAngle); //��ݼ��� ������ ���� �� ���� ����
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
                    // �������̰� 5�� �ϰ�� ���� 0.75�� ����
                    // marginOfErrorAngle = 20f; ���� 2���������� �������� ��� (** pinUnlockDistance�� ���� �޶���)
                    // marginOfErrorAngle = 40f; ���� 4���������� �������� ���
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
            //������ �������� 90�� �ȴ����� 0��
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
            PuzzleCamera.Priority = 11; // ī�޶� Ȱ��ȭ
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
            PuzzleCamera.Priority = 9; // ī�޶� ��Ȱ��ȭ
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
        //��乮 Interact �� Sound �߰�
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
