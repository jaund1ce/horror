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
    public Transform insidePosition; // ĳ��� ���� ��ġ

    private bool isPlayerInside = false; // �÷��̾ ĳ��� ���ο� �ִ��� ����

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
            // �÷��̾ ĳ��� �ۿ��� E Ű�� ������ �� ���� ��
            EnterCabinet();
        }
        else if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            // �÷��̾ ĳ��� �ȿ��� E Ű�� ������ �� ����
            ExitCabinet();
        }
    }

    private void EnterCabinet()
    {
        isOpen = true;
        ToggleDoor(); // �� ����

        Debug.Log("ĳ��ֿ� ���� �� �÷��̾� ��ġ: " + player.position);

        // �÷��̾� ��ġ�� ���η� �̵�
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        Debug.Log("ĳ��ֿ� �� �� �÷��̾� ��ġ: " + player.position);

        isPlayerInside = true;
        StartCoroutine(CloseDoorAfterDelay(1f)); // 1�� �Ŀ� �� �ݱ�
    }

    private void ExitCabinet()
    {
        isOpen = true;
        ToggleDoor(); // �� ����

        // �÷��̾ ĳ��� ������ �̵�
        Vector3 exitPosition = transform.position + transform.forward * 2f; // ĳ��� �������� �̵�
        player.position = exitPosition;

        isPlayerInside = false;
        StartCoroutine(CloseDoorAfterDelay(1f)); // 1�� �Ŀ� �� �ݱ�
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

    private System.Collections.IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isOpen)
        {
            ToggleDoor(); // �� �ݱ�
        }
    }
}
