using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeskHide : MonoBehaviour, IHideable
{
    public Transform player; // 플레이어 Transform
    public Transform underDeskPosition; // 책상 밑 위치
    public Transform inFrontOfDeskPosition; // 책상 앞 위치
    public CharacterController characterController; // 플레이어 CharacterController
    public float interactDistance = 3f; // 상호작용 거리

    private bool isUnderDesk = false; // 플레이어가 책상 밑에 있는지 여부

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!isUnderDesk)
            {
                OnHide();
            }
            else
            {
                OnExit();
            }
        }
    }

    public void OnHide()
    {

        Debug.Log("Hiding under the desk...");

        if (characterController != null)
        {
            characterController.enabled = false; // 캐릭터 컨트롤러 비활성화
        }

        player.position = underDeskPosition.position;
        player.rotation = underDeskPosition.rotation;

        isUnderDesk = true;
    }

 

    public void OnExit()
    {
        Debug.Log("Exiting to the front of the desk...");

        // 책상 앞의 위치로 즉시 이동
        player.position = inFrontOfDeskPosition.position;
        player.rotation = inFrontOfDeskPosition.rotation;

        if (characterController != null)
        {
            characterController.enabled = true; // 캐릭터 컨트롤러 활성화
        }

        isUnderDesk = false;
    }
}
