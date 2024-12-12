using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeskHide : MonoBehaviour, IHideable, IInteractable
{
    public GameObject player; // �÷��̾� Transform
    public Transform underDeskPosition; // å�� �� ��ġ
    public Transform inFrontOfDeskPosition; // å�� �� ��ġ
    public CharacterController characterController; // �÷��̾� CharacterController
    public float interactDistance = 3f; // ��ȣ�ۿ� �Ÿ�

    private bool isUnderDesk = false; // �÷��̾ å�� �ؿ� �ִ��� ����
    private bool isPlayerNear = false;

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.transform.position, transform.position);
    //    if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (!isUnderDesk)
    //        {
    //            OnHide();
    //        }
    //        else
    //        {
    //            OnExit();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            isPlayerNear = false;
        }
    }

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
        if (!isPlayerNear) return;

        if (!isUnderDesk) OnHide();
        else OnExit();
    }

    public string GetInteractPrompt()
    {
        if (isUnderDesk) return "Out";
        else if (!isPlayerNear) return "Get Near";
        else if (isPlayerNear) return "Hide";
        else return "";
    }
}
