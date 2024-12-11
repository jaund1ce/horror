using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]//직렬화를 통해서 json을 사용가능하게 변경, json
public class ItemData
{
    public int ID;
    public string ItemNameEng;
    public string ItemNameKor;
    public ItemType ItemType; //int로 변경 -> enum 형이 가능한가?
    public string ItemEffect;// 불필요
    public int ItemHealHealth;
    public int ItemHealMental;
    public string ItemDescription;
    public bool stackable;

    public Sprite ItemImage;
}

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/SO_Item")]
public class ItemSO : ScriptableObject
{
    public ItemData itemData;
}
