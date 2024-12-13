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
        this.amount = -1;

        this.slotIndex = index;
        this.quickslotIndex = -1;
    }

    public void ResetData()
    {
        this.ItemData = null;
        this.amount = -1;

        this.slotIndex = -1;
        this.quickslotIndex = -1;
    }
}

public class PlayerInventoryData : MonoBehaviour
{
    public InventoryData[] inventoryDatas = new InventoryData[15];//�κ��丮ĭ�� 15�� Ŭ������ new�� ���� �ϴ� ��� ������ �̸� �Ҵ����ְ� �� ���� ���� null�̴�.

    private void Awake()
    {
        for (int i = 0; i < inventoryDatas.Length; i++)//�Ҵ� �� �⺻ �� ����
        {
            inventoryDatas[i] = new InventoryData(i);
        }
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
    }

    private void CheckEmpty(ItemData itemData)
    {
        foreach(InventoryData item in inventoryDatas)
        {
            if(item.ItemData == null)
            {
                item.ItemData = itemData;
                item.amount = 1;
                return;
            }
        }
    }

    public void CheckStack(ItemData itemData)
    {
        foreach (InventoryData item in inventoryDatas)
        {
            if (item == null) continue;
            if (item.ItemData == null) continue;
            if (item.ItemData.itemSO == itemData.itemSO)
            {
                item.amount += 1;
                return;
            }
        }

        CheckEmpty(itemData);
    }
}

