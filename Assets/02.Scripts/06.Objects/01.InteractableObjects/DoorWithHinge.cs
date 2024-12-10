using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithHinge : MonoBehaviour
{
    private HingeJoint hinge;
    private bool isOpen = false; // �� ����
    public float openAngle = -90f; // �� ���� ����
    public float closeAngle = 0f;  // �� ���� ����
    public float speed = 5f; // �� ������ �ӵ�
    public float openRange = 3f; // ���� �� �� �ִ� �ִ� �Ÿ�
    public Transform player; // �÷��̾��� Transform (������ ���� ����)

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
        spring.spring = speed; // ���� ������ �ӵ�
        spring.damper = 1f; // ���谪
        spring.targetPosition = isOpen ? openAngle : closeAngle; // ��ǥ ����
        hinge.spring = spring;
        hinge.useSpring = true;

        Debug.Log(isOpen ? "���� ���Ƚ��ϴ�!" : "���� �������ϴ�!");
    }
}