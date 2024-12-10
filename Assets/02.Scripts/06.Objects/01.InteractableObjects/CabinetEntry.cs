using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetEntry : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform (������ ����)
    public Transform insidePosition; // ĳ��� ���� ��ġ
    public CharacterController characterController; // �÷��̾��� CharacterController
    private bool isPlayerInside = false; // �÷��̾ ĳ��� ���ο� �ִ��� ����

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
        Debug.Log("ĳ��ֿ� ���� �õ�");

        // ĳ��� ���η� �÷��̾� �̵�
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        // �÷��̾� �̵� ���
        if (characterController != null)
        {
            characterController.enabled = false; // �̵� ��Ȱ��ȭ
        }

        isPlayerInside = true;
        Debug.Log("�÷��̾ ĳ��� ���η� �̵��߽��ϴ�.");
    }

    private void ExitCabinet()
    {
        Debug.Log("ĳ��ֿ��� ������ �õ�");

        // �÷��̾� ĳ��� ������ �̵�
        Vector3 exitPosition = transform.position + transform.forward * 2f;
        player.position = exitPosition;

        // �÷��̾� �̵� Ȱ��ȭ
        if (characterController != null)
        {
            characterController.enabled = true; // �̵� Ȱ��ȭ
        }

        isPlayerInside = false;
        Debug.Log("�÷��̾ ĳ��ֿ��� ���Խ��ϴ�.");
    }
}
