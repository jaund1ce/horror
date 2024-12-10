using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetEntry : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform (������ ����)
    public Transform insidePosition; // ĳ��� ���� ��ġ
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

        // �÷��̾� ��ġ�� ���η� �̵�
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        isPlayerInside = true;
        Debug.Log("�÷��̾ ĳ��� ���η� �̵��߽��ϴ�.");
    }

    private void ExitCabinet()
    {
        Debug.Log("ĳ��ֿ��� ������ �õ�");

        // �÷��̾ ĳ��� ������ �̵�
        Vector3 exitPosition = transform.position + transform.forward * 2f;
        player.position = exitPosition;

        isPlayerInside = false;
        Debug.Log("�÷��̾ ĳ��ֿ��� ���Խ��ϴ�.");
    }
}
