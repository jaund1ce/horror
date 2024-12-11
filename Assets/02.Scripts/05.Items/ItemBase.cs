using System;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;

public class ItemBase : MonoBehaviour,IInteractable
{
    public ItemSO itemSO;
    private PlayerInventoryData inventoryslotcontroller;

    private void Start()
    {
        inventoryslotcontroller = MainGameManager.Instance.Player.playerInventoryData;
    }

    public string GetInteractPrompt()
    {
        string str = $"{itemSO.itemData.ItemNameEng}";
        return str;
    }

    public void OnInteract()
    {
        //GameManger.instance.Player.Inventorydata.AddItem(itemSO);
        inventoryslotcontroller.AddItem(itemSO);
        gameObject.SetActive(false);
    }
}
