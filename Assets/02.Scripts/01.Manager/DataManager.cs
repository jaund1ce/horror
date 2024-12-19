using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
   private ItemData itemSO;
   //public List<ItemSO> AllItems = new List<ItemSO>();

    public ItemData ItemSO
   {

    get { 
            if(itemSO == null)
            {
                GetData();
            }
            return itemSO; 
        }

    }

    protected override void Awake()
    {
        base.Awake();
        //LoadItemData();

    }

    //private void LoadItemData()
    //{
    //    TextAsset jsonData = ResourceManager.Instance.LoadAsset<TextAsset>("itemData", eAssetType.Data);

    //    if (jsonData != null)
    //    {
    //        MetaItemDataList metaList = JsonUtility.FromJson<MetaItemDataList>(jsonData.text);
    //    }
    //}

   

    private void GetData()
    {
        var json = ResourceManager.Instance.LoadAsset<TextAsset>("ItemSO", eAssetType.Data);
        itemSO = JsonUtility.FromJson<ItemData>(json.text);
    }
}
