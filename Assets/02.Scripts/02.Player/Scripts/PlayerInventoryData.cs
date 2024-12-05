using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    public ItemData ItemData;
    public int amount;

    public int slotIndex;
    public int quickslotIndex;

    public InventoryData(int index) //�⺻ ������
    {
        this.ItemData = null;
        this.amount = 0;

        this.slotIndex = index;
        this.quickslotIndex = -1;
}
}

public class PlayerInventoryData : MonoBehaviour
{
    public InventoryData[] inventoryDatas = new InventoryData[15];//�κ��丮ĭ�� 15�� Ŭ������ new�� ���� �ϴ� ��� ������ �̸� �Ҵ����ְ� �� ���� ���� null�̴�.

    private void Start()
    {
        for (int i = 0; i < inventoryDatas.Length; i++)//�Ҵ� �� �⺻ �� ����
        {
            inventoryDatas[i] = new InventoryData(i);
        }
    }

    public void AddItem(ItemSO itemData)
    {
        if (itemData.itemData.stackable)
        {
            CheckStack(itemData);
        }
        else
        {
            CheckEmpty(itemData);
        }
    }

    private void CheckEmpty(ItemSO itemData)
    {
        foreach(InventoryData item in inventoryDatas)
        {
            if(item.ItemData == null)
            {
                item.ItemData = itemData.itemData;
                item.amount = 1;
                return;
            }
        }
    }

    public void CheckStack(ItemSO itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item == null) continue;
            if(item.ItemData == itemData.itemData)
            {
                item.amount += 1;
                return;
            }
        }

        CheckEmpty(itemData);
    }
}

