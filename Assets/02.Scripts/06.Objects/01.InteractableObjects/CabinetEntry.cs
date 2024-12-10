using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetEntry : MonoBehaviour
{
    public Transform player; // 플레이어 Transform (씬에서 연결)
    public Transform insidePosition; // 캐비닛 내부 위치
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

        // 플레이어 위치를 내부로 이동
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        isPlayerInside = true;
        Debug.Log("플레이어가 캐비닛 내부로 이동했습니다.");
    }

    private void ExitCabinet()
    {
        Debug.Log("캐비닛에서 나오기 시도");

        // 플레이어를 캐비닛 밖으로 이동
        Vector3 exitPosition = transform.position + transform.forward * 2f;
        player.position = exitPosition;

        isPlayerInside = false;
        Debug.Log("플레이어가 캐비닛에서 나왔습니다.");
    }
}
