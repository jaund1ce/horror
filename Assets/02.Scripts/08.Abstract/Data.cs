using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataBase<T>
{
    public int index;

    public abstract void SetData(T metaData);
}

public abstract class DataBaseList<T1, T2, T3>
{
    public Dictionary<T1, T2> datas = new Dictionary<T1, T2>();

    public abstract void SetData(List<T3> metaDataList);
}

[System.Serializable]
public class ItemDataes : DataBase<MetaItemData>
{
    public string name;
    public string KOR_Name;

    public override void SetData(MetaItemData metaItemData)
    {
        this.index = metaItemData.index;
        this.name = metaItemData.name;
        this.KOR_Name = metaItemData.KOR_Name;
    }
}

[System.Serializable]
public class ItemDataList : DataBaseList<string, ItemDataes, MetaItemData>
{
    public override void SetData(List<MetaItemData> metaItemDatas)
    {
        datas = new Dictionary<string, ItemDataes>(metaItemDatas.Count);

        metaItemDatas.ForEach(obj =>
        {
            ItemDataes item = new ItemDataes();
            item.SetData(obj);
            datas.Add(item.name, item);
        });
    }
}



