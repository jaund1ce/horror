using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetEntry : MonoBehaviour, IInteractable
{
    public Transform cabinetDoor; // 캐비닛 문 Transform
    public float doorOpenAngle = 90f; // 문 열림 각도
    public float doorSpeed = 2f; // 문 열림 속도

    [SerializeField]private bool isDoorOpen = false; // 문이 열렸는지 여부


    //public void OnHide()
    //{
    //    Debug.Log("Player is hiding inside the cabinet...");

    //    // 캐릭터 이동 처리
    //    if (characterController != null)
    //    {            
    //        characterController.enabled = false; // 캐릭터 컨트롤러 비활성화
    //    }

    //    // 캐비닛 내부 위치로 이동
    //    player.transform.position = insidePosition.position;
    //    player.transform.rotation = insidePosition.rotation;

    //    isPlayerInside = true;
    //}

    //public void OnExit()
    //{
    //    Debug.Log("Player is exiting the cabinet...");

    //    // 캐비닛 외부 위치로 이동
    //    player.transform.position = outsidePosition.position;
    //    player.transform.rotation = outsidePosition.rotation;

    //    // 캐릭터 컨트롤러 활성화
    //    if (characterController != null)
    //    {
    //        characterController.enabled = true;
    //    }

    //    isPlayerInside = false;

    //    // 문 Collider 활성화
    //    if (cabinetDoorCollider != null)
    //    {
    //        cabinetDoorCollider.enabled = true;
    //    }
    //}

    private IEnumerator OpenDoor()
    {
        Debug.Log("Opening cabinet door...");

        isDoorOpen = true;

        Quaternion initialRotation = cabinetDoor.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            Debug.Log($"opening... {elapsedTime} {cabinetDoor.rotation}");
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.rotation = Quaternion.Slerp(cabinetDoor.rotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.rotation = targetRotation;

        //// 문 Collider 비활성화
        //if (cabinetDoorCollider != null)
        //{
        //    cabinetDoorCollider.enabled = false;
        //}

        Debug.Log("Cabinet door opened.");
    }

    private IEnumerator CloseDoor()
    {
        Debug.Log("Closing cabinet door...");

        isDoorOpen = false;

        Quaternion initialRotation = cabinetDoor.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.localRotation = targetRotation;

        Debug.Log("Cabinet door closed.");
    }

    public void OnInteract()
    {
        if (!isDoorOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    public string GetInteractPrompt()
    {
        if (!isDoorOpen) return "Open";
        else return "Close";
    }
}