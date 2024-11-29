using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumUtil<T>
{
    public static T Parse(string s)
    {
        return (T)Enum.Parse(typeof(T), s);
    }
}

public class ItemBase : MonoBehaviour,IInteractable
{
    public ItemSO itemSO;
    private void init()//아이템 생성 완료 시 실행
    {
        //itemType = (ItemType)Enum.Parse(typeof(ItemType),itemSO.itemData.ItemType);
    }

    public void HideInteractUI()
    {
        throw new NotImplementedException();
    }

    public void OnInteract()
    {
        throw new NotImplementedException();
    }

    public void ShowInteractUI()
    {
        throw new NotImplementedException();
    }

}
