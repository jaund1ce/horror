using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithHinge : MonoBehaviour, IInteractable
{
    public Transform hinge; // 문 힌지
    public float openAngle = -90f; // 문 열리는 각도
    public float closeAngle = 0f; // 문 닫히는 각도
    public float openSpeed = 5f; // 문 열림 속도
    public float pushForce = 2f;
    private Rigidbody doorRb;
    public Collider interactionCollider; // 플레이어 감지를 위한 콜라이더

    private bool isOpen = false; // 문이 열렸는지 여부
    //private bool isPlayerNear = false; // 플레이어가 근처에 있는지 여부

    private void Start()
    {
        doorRb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController character = other.GetComponent<CharacterController>();

            if (character != null)
            {
                // 문의 회전 방향에 따라 밀어내는 벡터 계산
                Vector3 pushDirection = doorRb.transform.right; // 문의 Local X축 방향으로 밀기
                pushDirection.y = 0; // Y축 이동 방지

                // 캐릭터의 위치를 미세하게 이동 (밀어내기)
                character.Move(pushDirection * pushForce * Time.deltaTime);
            }
        }
    }


    public void OnInteract()
    {
        //if (!isPlayerNear) return;

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
