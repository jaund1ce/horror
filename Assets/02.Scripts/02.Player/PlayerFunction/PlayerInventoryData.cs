using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public int ItemID; // ItemSO�� ID
    public int amount; // ������ ����
    public int slotIndex; // ���� �ε���
    public int quickslotIndex; // ������ �ε���

        

    // ������ȭ �ʵ�: ���� ItemSO ����
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
        this.ItemID = itemData.itemSO.ID; // ������: ItemSO�� ID ����
        this.amount = quantity;
    }

    public void ResetData()
    {
        this.ItemData = null;
        this.ItemID = -1;
        this.amount = 0;
        this.slotIndex = -1;
        this.quickslotIndex = -1; // ������: ������ �ε��� �ʱ�ȭ �߰�
    }
}

public class PlayerInventoryData : MonoBehaviour
{
    public InventoryData[] inventoryDatas = new InventoryData[16]; // �κ��丮ĭ�� 15�� Ŭ������ new�� �����ϴ� ��� ������ �̸� �Ҵ����ְ� �� ���� ���� null�̴�.

    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            return;
        }

        for (int i = 0; i < 16; i++) // �Ҵ� �� �⺻ �� ����
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
                item.SetItem(itemData, itemData.itemSO.ItemDropAmount); // ������: SetItem���� ID�� ������ ����
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
            DataManager.Instance.InventoryData[i] = inventoryDatas[i]; // ����ȭ
        }
    }



}
