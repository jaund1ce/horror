using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetEntry : MonoBehaviour, IInteractable
{
    public Transform cabinetDoor; // ĳ��� �� Transform
    public float doorOpenAngle = 90f; // �� ���� ����
    public float doorSpeed = 2f; // �� ���� �ӵ�

    [SerializeField]private bool isDoorOpen = false; // ���� ���ȴ��� ����


    //public void OnHide()
    //{
    //    Debug.Log("Player is hiding inside the cabinet...");

    //    // ĳ���� �̵� ó��
    //    if (characterController != null)
    //    {            
    //        characterController.enabled = false; // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
    //    }

    //    // ĳ��� ���� ��ġ�� �̵�
    //    player.transform.position = insidePosition.position;
    //    player.transform.rotation = insidePosition.rotation;

    //    isPlayerInside = true;
    //}

    //public void OnExit()
    //{
    //    Debug.Log("Player is exiting the cabinet...");

    //    // ĳ��� �ܺ� ��ġ�� �̵�
    //    player.transform.position = outsidePosition.position;
    //    player.transform.rotation = outsidePosition.rotation;

    //    // ĳ���� ��Ʈ�ѷ� Ȱ��ȭ
    //    if (characterController != null)
    //    {
    //        characterController.enabled = true;
    //    }

    //    isPlayerInside = false;

    //    // �� Collider Ȱ��ȭ
    //    if (cabinetDoorCollider != null)
    //    {
    //        cabinetDoorCollider.enabled = true;
    //    }
    //}

    private IEnumerator OpenDoor()
    {
        Debug.Log("Opening cabinet door...");

        isDoorOpen = true;

        Quaternion initialRotation = cabinetDoor.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            Debug.Log($"opening... {elapsedTime} {cabinetDoor.rotation}");
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.rotation = Quaternion.Slerp(cabinetDoor.rotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.rotation = targetRotation;

        //// �� Collider ��Ȱ��ȭ
        //if (cabinetDoorCollider != null)
        //{
        //    cabinetDoorCollider.enabled = false;
        //}

        Debug.Log("Cabinet door opened.");
    }

    private IEnumerator CloseDoor()
    {
        Debug.Log("Closing cabinet door...");

        isDoorOpen = false;

        Quaternion initialRotation = cabinetDoor.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.localRotation = targetRotation;

        Debug.Log("Cabinet door closed.");
    }

    public void OnInteract()
    {
        if (!isDoorOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    public string GetInteractPrompt()
    {
        if (!isDoorOpen) return "Open";
        else return "Close";
    }
}