using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeskHide : MonoBehaviour, IInteractable
{
    public GameObject player; // 플레이어 Transform
    public Transform underDeskPosition; // 책상 밑 위치
    public Transform inFrontOfDeskPosition; // 책상 앞 위치
    public CharacterController characterController; // 플레이어 CharacterController

    private bool isUnderDesk = false; // 플레이어가 책상 밑에 있는지 여부


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
        if (player == null) player = MainGameManager.Instance.Player.gameObject;

        if (!isUnderDesk) OnHide();
        else OnExit();
    }

    public string GetInteractPrompt()
    {
        if (isUnderDesk) return "Out";
        else return "Hide";
    }
}
