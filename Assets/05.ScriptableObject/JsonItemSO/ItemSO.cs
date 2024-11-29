using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]//직렬화를 통해서 json을 사용가능하게 변경, json
public class JsonItemData
{
    public object ID;
    public string ItemNameEng;
    public string ItemNameKor;
    public string ItemType;
    public string ItemEffect;
    public int ItemHealHealth;
    public int ItemHealMental;
    public string ItemDescription;
}

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/SO_Item")]
public class ItemSO : ScriptableObject
{
    public JsonItemData itemData;
}
