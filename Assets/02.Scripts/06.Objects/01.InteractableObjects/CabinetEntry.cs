using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetEntry : MonoBehaviour
{
    public Transform player; // 플레이어 Transform (씬에서 연결)
    public Transform insidePosition; // 캐비닛 내부 위치
    public CharacterController characterController; // 플레이어의 CharacterController
    private bool isPlayerInside = false; // 플레이어가 캐비닛 내부에 있는지 여부

    void Update()
    {
        if (!isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            EnterCabinet();
        }
        else if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            ExitCabinet();
        }
    }

    private void EnterCabinet()
    {
        Debug.Log("캐비닛에 들어가기 시도");

        // 캐비닛 내부로 플레이어 이동
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        // 플레이어 이동 잠금
        if (characterController != null)
        {
            characterController.enabled = false; // 이동 비활성화
        }

        isPlayerInside = true;
        Debug.Log("플레이어가 캐비닛 내부로 이동했습니다.");
    }

    private void ExitCabinet()
    {
        Debug.Log("캐비닛에서 나오기 시도");

        // 플레이어 캐비닛 밖으로 이동
        Vector3 exitPosition = transform.position + transform.forward * 2f;
        player.position = exitPosition;

        // 플레이어 이동 활성화
        if (characterController != null)
        {
            characterController.enabled = true; // 이동 활성화
        }

        isPlayerInside = false;
        Debug.Log("플레이어가 캐비닛에서 나왔습니다.");
    }
}
