using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithHinge : MonoBehaviour, IInteractable
{
    public Transform hinge; // �� ����
    public float openAngle = -90f; // �� ������ ����
    public float closeAngle = 0f; // �� ������ ����
    public float openSpeed = 5f; // �� ���� �ӵ�
    public float pushForce = 2f;
    private Rigidbody doorRb;
    public Collider interactionCollider; // �÷��̾� ������ ���� �ݶ��̴�

    private bool isOpen = false; // ���� ���ȴ��� ����
    //private bool isPlayerNear = false; // �÷��̾ ��ó�� �ִ��� ����

    private void Start()
    {
        doorRb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController character = other.GetComponent<CharacterController>();

            if (character != null)
            {
                // ���� ȸ�� ���⿡ ���� �о�� ���� ���
                Vector3 pushDirection = doorRb.transform.right; // ���� Local X�� �������� �б�
                pushDirection.y = 0; // Y�� �̵� ����

                // ĳ������ ��ġ�� �̼��ϰ� �̵� (�о��)
                character.Move(pushDirection * pushForce * Time.deltaTime);
            }
        }
    }


    public void OnInteract()
    {
        //if (!isPlayerNear) return;

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openAngle : closeAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        float currentAngle = hinge.localEulerAngles.y;
        if (currentAngle > 180) currentAngle -= 360;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * openSpeed);
            hinge.localEulerAngles = new Vector3(0, currentAngle, 0);
            yield return null;
        }

        hinge.localEulerAngles = new Vector3(0, targetAngle, 0);
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "Close" : "Open";
    }
}
