using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetWithHinge : MonoBehaviour
{
    private HingeJoint hinge;
    private bool isOpen = false; // 캐비닛 문 상태
    public float openAngle = -90f; // 캐비닛 열림 각도
    public float closeAngle = 0f;  // 캐비닛 닫힘 각도
    public float speed = 5f; // 문 열리는 속도
    public float openRange = 5f; // 캐비닛을 열 수 있는 최대 거리
    public Transform player; // 플레이어 Transform (씬에서 연결)
    public Transform insidePosition; // 캐비닛 내부 위치

    private bool isPlayerInside = false; // 플레이어가 캐비닛 내부에 있는지 여부

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        if (hinge != null)
        {
            JointLimits limits = hinge.limits;
            limits.min = openAngle;
            limits.max = closeAngle;
            hinge.limits = limits;
            hinge.useLimits = true;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (!isPlayerInside && distance <= openRange && Input.GetKeyDown(KeyCode.E))
        {
            // 플레이어가 캐비닛 밖에서 E 키를 눌렀을 때 열고 들어감
            EnterCabinet();
        }
        else if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            // 플레이어가 캐비닛 안에서 E 키를 눌렀을 때 나옴
            ExitCabinet();
        }
    }

    private void EnterCabinet()
    {
        isOpen = true;
        ToggleDoor(); // 문 열기

        Debug.Log("캐비닛에 들어가기 전 플레이어 위치: " + player.position);

        // 플레이어 위치를 내부로 이동
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        Debug.Log("캐비닛에 들어간 후 플레이어 위치: " + player.position);

        isPlayerInside = true;
        StartCoroutine(CloseDoorAfterDelay(1f)); // 1초 후에 문 닫기
    }

    private void ExitCabinet()
    {
        isOpen = true;
        ToggleDoor(); // 문 열기

        // 플레이어를 캐비닛 밖으로 이동
        Vector3 exitPosition = transform.position + transform.forward * 2f; // 캐비닛 앞쪽으로 이동
        player.position = exitPosition;

        isPlayerInside = false;
        StartCoroutine(CloseDoorAfterDelay(1f)); // 1초 후에 문 닫기
    }

    private void ToggleDoor()
    {
        JointSpring spring = hinge.spring;
        spring.spring = speed; // 문이 열리는 속도
        spring.damper = 1f; // 감쇠값
        spring.targetPosition = isOpen ? openAngle : closeAngle; // 목표 각도
        hinge.spring = spring;
        hinge.useSpring = true;

        isOpen = !isOpen;
        Debug.Log(isOpen ? "문이 열렸습니다!" : "문이 닫혔습니다!");
    }

    private System.Collections.IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isOpen)
        {
            ToggleDoor(); // 문 닫기
        }
    }
}
