using System;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public int ItemID; // ItemSO의 ID
    public int amount; // 아이템 수량
    public int slotIndex; // 슬롯 인덱스
    public int quickslotIndex; // 퀵슬롯 인덱스

    // 비직렬화 필드: 실제 ItemSO 참조
    [NonSerialized]
    public ItemData ItemData;

    public InventoryData(int index)
    {
        this.slotIndex = index;
        this.quickslotIndex = -1;
        this.amount = 0;
        this.ItemID = -1;
    }

    public void SetItem(ItemData itemData, int quantity)
    {
        this.ItemData = itemData;
        this.ItemID = itemData.itemSO.ID; // 수정됨: ItemSO의 ID 저장
        this.amount = quantity;
        Debug.Log($"SetItem called: ItemID={ItemID}, Quantity={quantity}"); // 수정됨: 디버깅 추가
    }

    public void ResetData()
    {
        this.ItemData = null;
        this.ItemID = -1;
        this.amount = 0;
        this.slotIndex = -1;
        this.quickslotIndex = -1; // 수정됨: 퀵슬롯 인덱스 초기화 추가
    }
}

public class PlayerInventoryData : MonoBehaviour
{
    public InventoryData[] inventoryDatas = new InventoryData[15]; // 인벤토리칸이 15개 클래스를 new로 선언하는 경우 공간만 미리 할당해주고 그 내부 값은 null이다.

    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("DataManager.Instance is null during PlayerInventoryData initialization.");
            return;
        }

        for (int i = 0; i < inventoryDatas.Length; i++) // 할당 후 기본 값 적용
        {
            inventoryDatas[i] = new InventoryData(i);
        }

        SyncInventoryData();
    }

    public void AddItem(ItemData itemData)
    {
        Debug.Log($"Attempting to add item: {itemData.itemSO.ItemNameEng}"); // 수정됨: 디버깅 메시지 추가
        if (itemData.itemSO.Stackable)
        {
            CheckStack(itemData);
        }
        else
        {
            CheckEmpty(itemData);
        }

        SyncInventoryData();
    }

    private void CheckEmpty(ItemData itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item.ItemData == null)
            {
                item.SetItem(itemData, 1); // 수정됨: SetItem으로 ID와 데이터 설정
                Debug.Log($"Item added to empty slot: {item.slotIndex}"); // 수정됨: 디버깅 메시지 추가
                return;
            }
        }
        Debug.LogWarning("No empty slot available!"); // 수정됨: 빈 슬롯이 없을 때 경고 메시지 추가
    }

    public void CheckStack(ItemData itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item == null || item.ItemData == null) continue;
            if (item.ItemData.itemSO == itemData.itemSO)
            {
                item.amount += 1;
                Debug.Log($"Item stacked in slot: {item.slotIndex}, New Amount: {item.amount}"); // 수정됨: 디버깅 메시지 추가
                return;
            }
        }

        CheckEmpty(itemData);
    }

    public void SyncInventoryData()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("DataManager.Instance is null! Ensure DataManager is initialized before calling SyncInventoryData.");
            return;
        }

        if (DataManager.Instance.InventoryData == null)
        {
            Debug.LogError("DataManager.InventoryData is null! Ensure InventoryData is initialized in DataManager.");
            return;
        }

        Debug.Log("Syncing InventoryData with PlayerInventoryData...");
        for (int i = 0; i < inventoryDatas.Length; i++)
        {
            DataManager.Instance.InventoryData[i] = inventoryDatas[i]; // 동기화
            if (inventoryDatas[i].ItemData != null)
            {
                Debug.Log($"Slot {i} Synced: ItemID={inventoryDatas[i].ItemID}, Amount={inventoryDatas[i].amount}");
            }
            else
            {
                Debug.Log($"Slot {i} is empty during sync.");
            }
        }
    }

}
