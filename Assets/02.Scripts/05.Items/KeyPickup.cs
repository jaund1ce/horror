using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupRange = 3f; // 열쇠를 획득할 수 있는 최대 거리
    private bool isPlayerNearby = false; // 플레이어가 범위 안에 있는지 확인
    private Transform player; // 플레이어의 Transform

    void Update()
    {
        // 플레이어가 범위 내에 있을 때만 E키로 획득 가능
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

        // 플레이어가 열쇠를 바라보고 있는지 확인
        Vector3 directionToKey = (transform.position - player.position).normalized;
        float distanceToKey = Vector3.Distance(transform.position, player.position);

        // 플레이어의 시선과 열쇠 방향의 각도 확인
        if (distanceToKey <= pickupRange && Vector3.Dot(player.forward, directionToKey) > 0.8f)
        {
            // 열쇠 획득
            LockedDoorWithHinge door = FindObjectOfType<LockedDoorWithHinge>();
            if (door != null)
            {
                door.AcquireKey();
            }
            Debug.Log("열쇠를 획득했습니다!");
            Destroy(gameObject); // 열쇠 삭제
        }
        else
        {
            Debug.Log("열쇠를 바라보아야만 획득할 수 있습니다.");
        }
    }
}
