using System;
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

    public void SetItem(ItemData itemData, int quantity)
    {
        this.ItemData = itemData;
        this.ItemID = itemData.itemSO.ID; // ������: ItemSO�� ID ����
        this.amount = quantity;
        Debug.Log($"SetItem called: ItemID={ItemID}, Quantity={quantity}"); // ������: ����� �߰�
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
    public InventoryData[] inventoryDatas = new InventoryData[15]; // �κ��丮ĭ�� 15�� Ŭ������ new�� �����ϴ� ��� ������ �̸� �Ҵ����ְ� �� ���� ���� null�̴�.

    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("DataManager.Instance is null during PlayerInventoryData initialization.");
            return;
        }

        for (int i = 0; i < inventoryDatas.Length; i++) // �Ҵ� �� �⺻ �� ����
        {
            inventoryDatas[i] = new InventoryData(i);
        }

        SyncInventoryData();
    }

    public void AddItem(ItemData itemData)
    {
        Debug.Log($"Attempting to add item: {itemData.itemSO.ItemNameEng}"); // ������: ����� �޽��� �߰�
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
                item.SetItem(itemData, 1); // ������: SetItem���� ID�� ������ ����
                Debug.Log($"Item added to empty slot: {item.slotIndex}"); // ������: ����� �޽��� �߰�
                return;
            }
        }
        Debug.LogWarning("No empty slot available!"); // ������: �� ������ ���� �� ��� �޽��� �߰�
    }

    public void CheckStack(ItemData itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item == null || item.ItemData == null) continue;
            if (item.ItemData.itemSO == itemData.itemSO)
            {
                item.amount += 1;
                Debug.Log($"Item stacked in slot: {item.slotIndex}, New Amount: {item.amount}"); // ������: ����� �޽��� �߰�
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
            DataManager.Instance.InventoryData[i] = inventoryDatas[i]; // ����ȭ
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
