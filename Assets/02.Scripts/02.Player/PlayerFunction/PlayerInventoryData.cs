using System;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public int ItemID; // ItemSO의 ID
    public int Amount; // 아이템 수량
    public int SlotIndex; // 슬롯 인덱스
    public int QuickslotIndex; // 퀵슬롯 인덱스

    // 비직렬화 필드: 실제 ItemSO 참조
    [NonSerialized]
    public ItemData ItemData;

    public InventoryData(int index)
    {
        this.SlotIndex = index;
        this.QuickslotIndex = -1;
        this.Amount = 0;
        this.ItemID = -1;
    }

    public TryUse Use(int amount) 
    {
        TryUse result;
        
        if (this.Amount >= amount)
        {
            UseItemQuantity(amount);
            result = TryUse.CanUse;
        }
        else result = TryUse.CanNotUse;

        if (this.Amount <= 0)
        {
            this.ResetData();
            result = TryUse.ResetItem;
        }

        return result;
    }

    public void UseItemQuantity(int amount)
    {
        if (this.Amount >= amount)
        {
            this.Amount -= amount;
        }
        else return;
    }

    public void SetItem(ItemData itemData, int quantity)
    {
        this.ItemData = itemData;
        this.ItemID = itemData.itemSO.ID; // 수정됨: ItemSO의 ID 저장
        this.Amount = quantity;
    }

    public void ResetData()
    {
        this.ItemData = null;
        this.ItemID = -1;
        this.Amount = 0;
        this.SlotIndex = -1;
        this.QuickslotIndex = -1; // 수정됨: 퀵슬롯 인덱스 초기화 추가
    }
}

public class PlayerInventoryData : MonoBehaviour
{
    public InventoryData[] inventoryDatas = new InventoryData[16];

    private void Awake()
    {
        if (DataManager.Instance == null) return;

        for (int i = 0; i < 16; i++)
        {
            inventoryDatas[i] = new InventoryData(i);
        }

        SyncInventoryData();
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData.itemSO.Stackable) CheckStack(itemData);
        else CheckEmpty(itemData);

        SyncInventoryData();
    }

    private void CheckEmpty(ItemData itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item == null || item.ItemData == null)
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
                item.Amount += itemData.itemSO.ItemDropAmount;
                return;
            }
        }

        CheckEmpty(itemData);
    }
    public void SyncInventoryData()
    {
        if (DataManager.Instance == null || DataManager.Instance.InventoryData == null) return;

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
