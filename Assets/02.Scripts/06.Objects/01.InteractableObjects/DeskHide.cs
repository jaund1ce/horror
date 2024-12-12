using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeskHide : MonoBehaviour, IHideable, IInteractable
{
    public GameObject player; // 플레이어 Transform
    public Transform underDeskPosition; // 책상 밑 위치
    public Transform inFrontOfDeskPosition; // 책상 앞 위치
    public CharacterController characterController; // 플레이어 CharacterController
    public float interactDistance = 3f; // 상호작용 거리

    private bool isUnderDesk = false; // 플레이어가 책상 밑에 있는지 여부
    private bool isPlayerNear = false;

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.transform.position, transform.position);
    //    if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (!isUnderDesk)
    //        {
    //            OnHide();
    //        }
    //        else
    //        {
    //            OnExit();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            isPlayerNear = false;
        }
    }

    public void OnHide()
    {

        Debug.Log("Hiding under the desk...");

        if (characterController != null)
        {
            characterController.enabled = false; // 캐릭터 컨트롤러 비활성화
        }

        player.transform.position = underDeskPosition.position;
        player.transform.rotation = underDeskPosition.rotation;

        isUnderDesk = true;
    }

 

    public void OnExit()
    {
        Debug.Log("Exiting to the front of the desk...");

        // 책상 앞의 위치로 즉시 이동
        player.transform.position = inFrontOfDeskPosition.position;
        player.transform.rotation = inFrontOfDeskPosition.rotation;

        if (characterController != null)
        {
            characterController.enabled = true; // 캐릭터 컨트롤러 활성화
        }

        isUnderDesk = false;
    }

    public void OnInteract()
    {
        if (!isPlayerNear) return;

        if (!isUnderDesk) OnHide();
        else OnExit();
    }

    public string GetInteractPrompt()
    {
        if (isUnderDesk) return "Out";
        else if (!isPlayerNear) return "Get Near";
        else if (isPlayerNear) return "Hide";
        else return "";
    }
}
