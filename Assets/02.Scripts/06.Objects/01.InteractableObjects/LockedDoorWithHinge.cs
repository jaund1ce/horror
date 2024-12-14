using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorWithHinge : MonoBehaviour, IInteractable
{
    public Transform hinge; // �� ����
    public float openAngle = -90f; // �� ������ ����
    public float closeAngle = 0f; // �� ������ ����
    public float openSpeed = 5f; // �� ���� �ӵ�
    public Collider interactionCollider; // �÷��̾� ������ ���� �ݶ��̴�
    //public bool isLocked = true; // �� ��� ����
    private bool isOpened = false; // ���� ���ȴ��� ����
    private bool isPlayerNear = false; // �÷��̾ ��ó�� �ִ��� ����

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
        Debug.Log("�� �� ����");
        StartCoroutine(RotateDoor(isOpened ? openAngle : closeAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        Debug.Log("������ �ڷ�ƾ ����");
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

    // AcquireKey �޼��� �߰�
    //public void AcquireKey()
    //{
    //    isLocked = false;
    //    Debug.Log("Key acquired! Door is now unlocked.");
    //}
}

