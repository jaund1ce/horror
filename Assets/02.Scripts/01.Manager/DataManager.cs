using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
   /*private ItemDataList itemDataList;
    
   public ItemDataList ItemData_List
    {

    get { 
            if(itemDataList == null)
            {
                GetData();
            }
            return itemDataList; 
        }

    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void GetData()
    {
        var json = ResourseManager,instance.LoadAsset<TextAsset>("ItemData", eAssetType.JsonData);
        temDataList = JsonUtility.FromJson<ItemDataList>(json.text);
    }*/
}
