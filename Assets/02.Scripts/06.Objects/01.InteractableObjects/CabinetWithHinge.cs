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

        if (distance <= openRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
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
}
