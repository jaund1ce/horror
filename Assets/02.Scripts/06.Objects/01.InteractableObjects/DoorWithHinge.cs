using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithHinge : MonoBehaviour, IInteractable
{
    public Transform hinge; // 문 힌지
    public float openAngle = -90f; // 문 열리는 각도
    public float closeAngle = 0f; // 문 닫히는 각도
    public float openSpeed = 5f; // 문 열림 속도
    public Collider interactionCollider; // 플레이어 감지를 위한 콜라이더

    private bool isOpen = false; // 문이 열렸는지 여부
    private bool isPlayerNear = false; // 플레이어가 근처에 있는지 여부

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player is near the door.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the door area.");
        }
    }

    public void OnInteract()
    {
        if (!isPlayerNear) return;

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openAngle : closeAngle));
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

    public string GetInteractPrompt()
    {
        return isOpen ? "Close" : "Open";
    }
}
