using System;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;

public class ItemBase : MonoBehaviour,IInteractable
{
    public ItemSO itemSO;
    public ItemData itemData;
    private PlayerInventoryData inventoryslotcontroller;

    private void Start()
    {
        inventoryslotcontroller = MainGameManager.Instance.Player.playerInventoryData;
    }

    public string GetInteractPrompt()
    {
        string str = $"{itemData.itemSO.ItemNameEng}";
        return str;
    }

    public void OnInteract()
    {
        //InventoryManager inventory = FindObjectOfType<InventoryManager>();
        //inventory.AddItem(itemSO);
        //GameManger.instance.Player.Inventorydata.AddItem(itemSO);
        inventoryslotcontroller.AddItem(itemData);
        gameObject.SetActive(false);
    }
}
