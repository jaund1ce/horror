using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorWithHinge : MonoBehaviour
{
    public Transform hinge;
    public float openAngle = -90f;
    public float closeAngle = 0f;
    public float openSpeed = 5f;
    public bool isLocked = true;

    private bool isDoorOpen = false;
    private bool playerNearby = false;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isLocked)
        {
            ToggleDoor();
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("Door unlocked!");
    }

    public void AcquireKey()
    {
        UnlockDoor(); // 키를 획득하면 문을 잠금 해제
    }

    private void ToggleDoor()
    {
        isDoorOpen = !isDoorOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isDoorOpen ? openAngle : closeAngle));
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
