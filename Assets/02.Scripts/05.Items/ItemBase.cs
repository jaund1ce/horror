using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour,IInteractable
{
    public ItemSO itemSO;
    public ItemData itemData;
    private PlayerInventoryData inventoryslotcontroller;

    private void Start()
    {
        inventoryslotcontroller = MainGameManager.Instance.Player.PlayerInventoryData;
    }

    public string GetInteractPrompt()
    {
        string str = $"{itemData.itemSO.ItemNameEng}";
        return str;
    }

    public void OnInteract()
    {
        inventoryslotcontroller.AddItem(itemData);
        gameObject.SetActive(false);
        SoundManger.Instance.MakeEnviormentSound("PickupObject", 1f);
    }
}
