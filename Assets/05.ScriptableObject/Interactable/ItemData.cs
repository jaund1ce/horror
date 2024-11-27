using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Interactable",menuName = "Data/SO_Item")]
public class ItemData : InteractableData
{
    public ItemType ItemType;
    public bool Stackable;
    public Image ItemImage;
    public string ItemDescription;
}
