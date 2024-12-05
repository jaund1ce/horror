using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour,IInteractable
{
    public ItemSO itemSO;
    public PlayerInventoryData inventoryslotcontroller;

    public void OnInteract()
    {
        //GameManger.instance.Player.Inventorydata.AddItem(itemSO);
        inventoryslotcontroller.AddItem(itemSO);
        gameObject.SetActive(false);
    }
}
