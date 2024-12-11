using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorWithHinge : MonoBehaviour
{
    private HingeJoint hinge;
    private bool isOpen = false; // �� ����
    public float openAngle = -90f; // �� ���� ����
    public float closeAngle = 0f;  // �� ���� ����
    public float speed = 5f; // �� ������ �ӵ�
    public float openRange = 3f; // ���� �� �� �ִ� �ִ� �Ÿ�
    public Transform player; // �÷��̾��� Transform (������ ���� ����)
    public Collider doorFrameCollider; // ��Ʋ�� Collider
    public bool hasKey = false; // ���� ���� ����

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        Collider doorCollider = GetComponent<Collider>();

        // ���� ��Ʋ�� �浹 ����
        if (doorFrameCollider != null && doorCollider != null)
        {
            Physics.IgnoreCollision(doorCollider, doorFrameCollider);
        }

        if (hinge != null)
        {
            JointLimits limits = hinge.limits;
            limits.min = openAngle;
            limits.max = closeAngle;
            hinge.limits = limits;
            hinge.useLimits = true;

            // �ʱ� ���� ����
            JointSpring spring = hinge.spring;
            spring.spring = speed;
            spring.damper = 1f;
            spring.targetPosition = closeAngle;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // �÷��̾ �� ���� �ȿ� �ְ� ���谡 ���� ���� ���� �ݱ�
            if (distance <= openRange && hasKey && Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
            }
            else if (distance <= openRange && !hasKey && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("���谡 �ʿ��մϴ�!");
            }
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (hinge != null)
        {
            JointSpring spring = hinge.spring;
            spring.spring = speed; // ���� ������ �ӵ�
            spring.damper = 1f; // ���谪
            spring.targetPosition = isOpen ? openAngle : closeAngle; // ��ǥ ����
            hinge.spring = spring;
            hinge.useSpring = true;

            Debug.Log(isOpen ? "���� ���Ƚ��ϴ�!" : "���� �������ϴ�!");
        }
    }

    public void AcquireKey()
    {
        hasKey = true;
        Debug.Log("���踦 ȹ���߽��ϴ�!");
    }
}
