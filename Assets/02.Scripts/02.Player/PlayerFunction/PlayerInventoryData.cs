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

    public InventoryData(int index) //기본 생성자
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
    public InventoryData[] inventoryDatas = new InventoryData[15];//인벤토리칸이 15개 클래스를 new로 선언 하는 경우 공간만 미리 할당해주고 그 내부 값은 null이다.

    private void Awake()
    {
        for (int i = 0; i < inventoryDatas.Length; i++)//할당 후 기본 값 적용
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

