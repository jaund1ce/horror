using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetWithHinge : MonoBehaviour
{
    private HingeJoint hinge;
    private bool isOpen = false; // ĳ��� �� ����
    public float openAngle = -90f; // ĳ��� ���� ����
    public float closeAngle = 0f;  // ĳ��� ���� ����
    public float speed = 5f; // �� ������ �ӵ�
    public float openRange = 5f; // ĳ����� �� �� �ִ� �ִ� �Ÿ�
    public Transform player; // �÷��̾� Transform (������ ����)

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
        spring.spring = speed; // ���� ������ �ӵ�
        spring.damper = 1f; // ���谪
        spring.targetPosition = isOpen ? openAngle : closeAngle; // ��ǥ ����
        hinge.spring = spring;
        hinge.useSpring = true;

        isOpen = !isOpen;
        Debug.Log(isOpen ? "���� ���Ƚ��ϴ�!" : "���� �������ϴ�!");
    }
}
