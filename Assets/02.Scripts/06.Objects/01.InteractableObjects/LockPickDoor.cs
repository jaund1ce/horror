using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LockPickDoor : PuzzleBase
{
    [Header("Door Settings")]
    [field: SerializeField] private Transform hinge; 
    private float openAngle = -90f; 
    private float closeAngle = 0f; 
    private float openSpeed = 5f; 
    public bool IsLocked = true;
    private bool isOpened = false;

    [Header("LcokPick Settings")]
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private Transform pin;
    [field: SerializeField] private Transform keyhole;
    [field: SerializeField] private Transform lockPickPoint; //락픽이 꽂혀져보이는 정확한 지점
    [field: SerializeField] private SoundClip Unlock;
    private float pinCenterPoint;
    private float keyholeCenterPoint;
    private float keyHoleTargetAngle;
    private float keyholeUnlockAngle = -90f;
    private float keyholeRotateSpeed = 2f;
    private float currentKeyholeAngle;

    [Header("Pin Settings")]
    [field: SerializeField] private SoundClip pinBreak;
    private float pinRotateSpeed = 20f;
    private float pinLifeTime = 2f;
    private float pinResetTime = 1f;
    private float pinShakeAmount = 3f;
    private float currentPinAngle;
    private float marginOfErrorAngle = 0.5f; // 정답 각도와의 오차 범위
    



    private void OnEnable()
    {
        hinge = this.transform;
    }

    private void Update()
    {
        if (!isUsingPuzzle) return;

        if (lockPickPoint != null) 
        {
            InitializePinPosition();
        }



    }

    private void InitializePinPosition() 
    {
        pin.position = lockPickPoint.position;
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
        base.EnterPuzzleView();
        //락픽 UI 띄우기
        isUsingPuzzle = true;
    }

    protected override void ExitPuzzleView()
    {
        base.ExitPuzzleView();
        //락픽 UI 제거
        isUsingPuzzle = false;
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
