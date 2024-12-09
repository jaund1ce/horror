using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithHinge : MonoBehaviour
{
    private HingeJoint hinge;
    private bool isOpen = false; // 문 상태
    public float openAngle = -90f; // 문 열림 각도
    public float closeAngle = 0f;  // 문 닫힘 각도
    public float speed = 5f; // 문 열리는 속도
    public float openRange = 3f; // 문을 열 수 있는 최대 거리
    public Transform player; // 플레이어의 Transform (씬에서 직접 연결)

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
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= openRange && Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
            }
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        JointSpring spring = hinge.spring;
        spring.spring = speed; // 문이 열리는 속도
        spring.damper = 1f; // 감쇠값
        spring.targetPosition = isOpen ? openAngle : closeAngle; // 목표 각도
        hinge.spring = spring;
        hinge.useSpring = true;

        Debug.Log(isOpen ? "문이 열렸습니다!" : "문이 닫혔습니다!");
    }
}