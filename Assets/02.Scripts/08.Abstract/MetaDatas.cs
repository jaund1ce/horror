using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MetaItemData
{
    public int index;
    public string name;
    public string KOR_Name;
    public int atk;
    public int def;
    public int str;
    public int crt;
}

[System.Serializable]
public class MetaItemDataList
{
    public List<MetaItemData> itemData;
}
