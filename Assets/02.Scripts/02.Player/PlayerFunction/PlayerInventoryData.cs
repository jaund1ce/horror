using System;
using Unity.VisualScripting;
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

    public int Use(int amount) 
    {
        int result;
        if (this.amount >= amount)
        {
            UseItemQuantity(amount);
            result = (int)TryUse.CanUse;
        }
        else result = (int)TryUse.CanNotUse;
        if (this.amount <= 0)
        {
            this.ResetData();
            result = (int)TryUse.ResetItem;
        }
        return result;
    }

    public void UseItemQuantity(int amount)
    {
        if (this.amount >= amount)
        {
            this.amount -= amount;
        }
        else return;
    }

    public void SetItem(ItemData itemData, int quantity)
    {
        this.ItemData = itemData;
        this.ItemID = itemData.itemSO.ID; // 수정됨: ItemSO의 ID 저장
        this.amount = quantity;
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
    public InventoryData[] inventoryDatas = new InventoryData[16]; // 인벤토리칸이 15개 클래스를 new로 선언하는 경우 공간만 미리 할당해주고 그 내부 값은 null이다.

    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            return;
        }

        for (int i = 0; i < 16; i++) // 할당 후 기본 값 적용
        {
            inventoryDatas[i] = new InventoryData(i);
        }

        SyncInventoryData();
    }

    public void AddItem(ItemData itemData)
    {
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
                item.SetItem(itemData, itemData.itemSO.ItemDropAmount); // 수정됨: SetItem으로 ID와 데이터 설정
                return;
            }
        }
    }

    public void CheckStack(ItemData itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item == null || item.ItemData == null) continue;
            if (item.ItemData.itemSO == itemData.itemSO)
            {
                item.amount += itemData.itemSO.ItemDropAmount;
                return;
            }
        }

        CheckEmpty(itemData);
    }
    public void SyncInventoryData()
    {
        if (DataManager.Instance == null || DataManager.Instance.InventoryData == null)
        {
            return;
        }

        if (DataManager.Instance.InventoryData.Length != inventoryDatas.Length)
        {
            Debug.LogError("InventoryData size mismatch!");
            return;
        }

        for (int i = 0; i < inventoryDatas.Length; i++)
        {
            DataManager.Instance.InventoryData[i] = inventoryDatas[i]; // 동기화
        }
    }



}
