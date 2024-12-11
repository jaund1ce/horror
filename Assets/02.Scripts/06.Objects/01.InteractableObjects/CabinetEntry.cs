using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHideable
{
    public void OnHide(); // 인터페이스 메서드 정의
    public void OnExit();
}

public class CabinetEntry : MonoBehaviour, IHideable
{
    public Transform player; // 플레이어 Transform
    public Transform insidePosition; // 캐비닛 내부 위치
    public Transform outsidePosition; // 캐비닛 외부 위치
    public Transform cabinetDoor; // 캐비닛 문 Transform
    public Collider cabinetDoorCollider; // 캐비닛 문 Collider
    public CharacterController characterController; // 플레이어 CharacterController
    public float interactDistance = 3f; // 상호작용 거리
    public float doorOpenAngle = 90f; // 문 열림 각도
    public float doorSpeed = 2f; // 문 열림 속도

    private bool isDoorOpen = false; // 문이 열렸는지 여부
    private bool isPlayerInside = false; // 플레이어가 캐비닛 내부에 있는지 여부

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDoorOpen)
            {
                StartCoroutine(OpenDoor());
            }
            else if (isDoorOpen && !isPlayerInside)
            {
                OnHide();
            }
            else if (isPlayerInside)
            {
                OnExit();
            }
        }
    }

    public void OnHide()
    {
        Debug.Log("Player is hiding inside the cabinet...");

        // 캐릭터 이동 처리
        if (characterController != null)
        {
            characterController.enabled = false; // 캐릭터 컨트롤러 비활성화
        }

        // 캐비닛 내부 위치로 이동
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        isPlayerInside = true;
    }

    public void OnExit()
    {
        Debug.Log("Player is exiting the cabinet...");

        // 캐비닛 외부 위치로 이동
        player.position = outsidePosition.position;
        player.rotation = outsidePosition.rotation;

        // 캐릭터 컨트롤러 활성화
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        isPlayerInside = false;

        // 문 Collider 활성화
        if (cabinetDoorCollider != null)
        {
            cabinetDoorCollider.enabled = true;
        }
    }

    private IEnumerator OpenDoor()
    {
        Debug.Log("Opening cabinet door...");

        isDoorOpen = true;

        Quaternion initialRotation = cabinetDoor.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.localRotation = targetRotation;

        // 문 Collider 비활성화
        if (cabinetDoorCollider != null)
        {
            cabinetDoorCollider.enabled = false;
        }

        Debug.Log("Cabinet door opened.");
    }
}