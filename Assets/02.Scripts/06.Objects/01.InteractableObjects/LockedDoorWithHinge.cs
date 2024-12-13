using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorWithHinge : MonoBehaviour, IInteractable
{
    public Transform hinge; // 문 힌지
    public float openAngle = -90f; // 문 열리는 각도
    public float closeAngle = 0f; // 문 닫히는 각도
    public float openSpeed = 5f; // 문 열림 속도
    public Collider interactionCollider; // 플레이어 감지를 위한 콜라이더
    //public bool isLocked = true; // 문 잠김 여부
    private bool isOpened = false; // 문이 열렸는지 여부
    private bool isPlayerNear = false; // 플레이어가 근처에 있는지 여부

    public event Action isOpen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player is near the locked door.");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the locked door area.");
        }
    }

    public void OnInteract()
    {
        if (!isPlayerNear) return;

        //if (isLocked)
        //{
        //    Debug.Log("The door is locked.");
        //    return;
        //}

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpened = !isOpened;
        if (isOpened == true)
        {
            isOpen?.Invoke();
        }

        StopAllCoroutines();
        Debug.Log("문 잘 열림");
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

    public string GetInteractPrompt()
    {
        //if (isLocked) return "Locked";
        return isOpened ? "Close" : "Open";
    }

    // AcquireKey 메서드 추가
    //public void AcquireKey()
    //{
    //    isLocked = false;
    //    Debug.Log("Key acquired! Door is now unlocked.");
    //}
}

