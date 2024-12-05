using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]//����ȭ�� ���ؼ� json�� ��밡���ϰ� ����, json
public class ItemData
{
    public int ID;
    public string ItemNameEng;
    public string ItemNameKor;
    public ItemType ItemType; //int�� ���� -> enum ���� �����Ѱ�?
    public string ItemEffect;// ���ʿ�
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
