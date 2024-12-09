using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupRange = 3f; // ���踦 ȹ���� �� �ִ� �ִ� �Ÿ�
    private bool isPlayerNearby = false; // �÷��̾ ���� �ȿ� �ִ��� Ȯ��
    private Transform player; // �÷��̾��� Transform

    void Update()
    {
        // �÷��̾ ���� ���� ���� ���� EŰ�� ȹ�� ����
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TryPickupKey();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            player = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            player = null;
        }
    }

    private void TryPickupKey()
    {
        if (player == null) return;

        // �÷��̾ ���踦 �ٶ󺸰� �ִ��� Ȯ��
        Vector3 directionToKey = (transform.position - player.position).normalized;
        float distanceToKey = Vector3.Distance(transform.position, player.position);

        // �÷��̾��� �ü��� ���� ������ ���� Ȯ��
        if (distanceToKey <= pickupRange && Vector3.Dot(player.forward, directionToKey) > 0.8f)
        {
            // ���� ȹ��
            LockedDoorWithHinge door = FindObjectOfType<LockedDoorWithHinge>();
            if (door != null)
            {
                door.AcquireKey();
            }
            Debug.Log("���踦 ȹ���߽��ϴ�!");
            Destroy(gameObject); // ���� ����
        }
        else
        {
            Debug.Log("���踦 �ٶ󺸾ƾ߸� ȹ���� �� �ֽ��ϴ�.");
        }
    }
}
