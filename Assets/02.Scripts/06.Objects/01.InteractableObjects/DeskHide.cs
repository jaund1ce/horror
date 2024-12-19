using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeskHide : MonoBehaviour, IInteractable
{
    public GameObject player; // �÷��̾� Transform
    public Transform underDeskPosition; // å�� �� ��ġ
    public Transform inFrontOfDeskPosition; // å�� �� ��ġ
    public CharacterController characterController; // �÷��̾� CharacterController

    private bool isUnderDesk = false; // �÷��̾ å�� �ؿ� �ִ��� ����


    public void OnHide()
    {

        Debug.Log("Hiding under the desk...");

        if (characterController != null)
        {
            characterController.enabled = false; // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        }

        player.transform.position = underDeskPosition.position;
        player.transform.rotation = underDeskPosition.rotation;

        isUnderDesk = true;
    }

    public void OnExit()
    {
        Debug.Log("Exiting to the front of the desk...");

        // å�� ���� ��ġ�� ��� �̵�
        player.transform.position = inFrontOfDeskPosition.position;
        player.transform.rotation = inFrontOfDeskPosition.rotation;

        if (characterController != null)
        {
            characterController.enabled = true; // ĳ���� ��Ʈ�ѷ� Ȱ��ȭ
        }

        isUnderDesk = false;
    }

    public void OnInteract()
    {
        if (player == null) player = MainGameManager.Instance.Player.gameObject;

        if (!isUnderDesk) OnHide();
        else OnExit();
    }

    public string GetInteractPrompt()
    {
        if (isUnderDesk) return "Out";
        else return "Hide";
    }
}
