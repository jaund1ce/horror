using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHideable
{
    public void OnHide(); // �������̽� �޼��� ����
    public void OnExit();
}

public class CabinetEntry : MonoBehaviour, IHideable, IInteractable
{
    public GameObject player; // �÷��̾� Transform
    public Transform insidePosition; // ĳ��� ���� ��ġ
    public Transform outsidePosition; // ĳ��� �ܺ� ��ġ
    public Transform cabinetDoor; // ĳ��� �� Transform
    public Collider cabinetDoorCollider; // ĳ��� �� Collider
    public CharacterController characterController; // �÷��̾� CharacterController
    public float interactDistance = 3f; // ��ȣ�ۿ� �Ÿ�
    public float doorOpenAngle = 90f; // �� ���� ����
    public float doorSpeed = 2f; // �� ���� �ӵ�

    [SerializeField]private bool isDoorOpen = false; // ���� ���ȴ��� ����
    [SerializeField] private bool isPlayerInside = false; // �÷��̾ ĳ��� ���ο� �ִ��� ����
    [SerializeField] private bool isPlayerNear = false;

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.position, transform.position);
    //    if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (!isDoorOpen)
    //        {
    //            StartCoroutine(OpenDoor());
    //        }
    //        else if (isDoorOpen && !isPlayerInside)
    //        {
    //            OnHide();
    //        }
    //        else if (isPlayerInside)
    //        {
    //            OnExit();
    //        }
    //    }
    //}

    private void GetPlayerData()
    {
        characterController = player.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = other.gameObject;
            GetPlayerData();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null;
        }
    }

    public void OnHide()
    {
        Debug.Log("Player is hiding inside the cabinet...");

        // ĳ���� �̵� ó��
        if (characterController != null)
        {            
            characterController.enabled = false; // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        }

        // ĳ��� ���� ��ġ�� �̵�
        player.transform.position = insidePosition.position;
        player.transform.rotation = insidePosition.rotation;

        isPlayerInside = true;
    }

    public void OnExit()
    {
        Debug.Log("Player is exiting the cabinet...");

        // ĳ��� �ܺ� ��ġ�� �̵�
        player.transform.position = outsidePosition.position;
        player.transform.rotation = outsidePosition.rotation;

        // ĳ���� ��Ʈ�ѷ� Ȱ��ȭ
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        isPlayerInside = false;

        // �� Collider Ȱ��ȭ
        if (cabinetDoorCollider != null)
        {
            cabinetDoorCollider.enabled = true;
        }
    }

    private IEnumerator OpenDoor()
    {
        Debug.Log("Opening cabinet door...");

        isDoorOpen = true;

        Quaternion initialRotation = cabinetDoor.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.localRotation = targetRotation;

        // �� Collider ��Ȱ��ȭ
        if (cabinetDoorCollider != null)
        {
            cabinetDoorCollider.enabled = false;
        }

        Debug.Log("Cabinet door opened.");
    }

    public void OnInteract()
    {
        if (!isPlayerNear) return;

        if (!isDoorOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else if (isDoorOpen && !isPlayerInside)
        {
            OnHide();
        }
        else if (isPlayerInside)
        {
            OnExit();
        }
    }

    public string GetInteractPrompt()
    {
        if (!isPlayerNear) return "Get Near";

        if (!isDoorOpen) return "Open";
        else if (isPlayerInside) return "Get Out";
        else if (isDoorOpen && isPlayerNear) return "Get Inside";
        else return "Err";
    }
}