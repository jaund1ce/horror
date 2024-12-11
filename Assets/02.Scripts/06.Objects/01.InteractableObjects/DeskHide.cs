using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeskHide : MonoBehaviour, IHideable
{
    public Transform player; // �÷��̾� Transform
    public Transform underDeskPosition; // å�� �� ��ġ
    public Transform inFrontOfDeskPosition; // å�� �� ��ġ
    public CharacterController characterController; // �÷��̾� CharacterController
    public float interactDistance = 3f; // ��ȣ�ۿ� �Ÿ�

    private bool isUnderDesk = false; // �÷��̾ å�� �ؿ� �ִ��� ����

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!isUnderDesk)
            {
                OnHide();
            }
            else
            {
                OnExit();
            }
        }
    }

    public void OnHide()
    {

        Debug.Log("Hiding under the desk...");

        if (characterController != null)
        {
            characterController.enabled = false; // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        }

        player.position = underDeskPosition.position;
        player.rotation = underDeskPosition.rotation;

        isUnderDesk = true;
    }

 

    public void OnExit()
    {
        Debug.Log("Exiting to the front of the desk...");

        // å�� ���� ��ġ�� ��� �̵�
        player.position = inFrontOfDeskPosition.position;
        player.rotation = inFrontOfDeskPosition.rotation;

        if (characterController != null)
        {
            characterController.enabled = true; // ĳ���� ��Ʈ�ѷ� Ȱ��ȭ
        }

        isUnderDesk = false;
    }
}
