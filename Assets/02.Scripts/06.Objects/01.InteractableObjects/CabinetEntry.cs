using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHideable
{
    public void OnHide(); // �������̽� �޼��� ����
    public void OnExit();
}

public class CabinetEntry : MonoBehaviour, IHideable
{
    public Transform player; // �÷��̾� Transform
    public Transform insidePosition; // ĳ��� ���� ��ġ
    public Transform outsidePosition; // ĳ��� �ܺ� ��ġ
    public Transform cabinetDoor; // ĳ��� �� Transform
    public Collider cabinetDoorCollider; // ĳ��� �� Collider
    public CharacterController characterController; // �÷��̾� CharacterController
    public float interactDistance = 3f; // ��ȣ�ۿ� �Ÿ�
    public float doorOpenAngle = 90f; // �� ���� ����
    public float doorSpeed = 2f; // �� ���� �ӵ�

    private bool isDoorOpen = false; // ���� ���ȴ��� ����
    private bool isPlayerInside = false; // �÷��̾ ĳ��� ���ο� �ִ��� ����

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
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
        player.position = insidePosition.position;
        player.rotation = insidePosition.rotation;

        isPlayerInside = true;
    }

    public void OnExit()
    {
        Debug.Log("Player is exiting the cabinet...");

        // ĳ��� �ܺ� ��ġ�� �̵�
        player.position = outsidePosition.position;
        player.rotation = outsidePosition.rotation;

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
}