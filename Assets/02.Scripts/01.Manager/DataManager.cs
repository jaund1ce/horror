using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
   private ItemData itemSO;
   public ItemDataList ItemData_List = new ItemDataList();

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

        TextAsset data = Resources.Load<TextAsset>("itemData");
        MetaItemDataList metaItemDataList = JsonUtility.FromJson<MetaItemDataList>(data.text);
        ItemData_List.SetData(metaItemDataList.itemData);
    }

    private void GetData()
    {
        var json = ResourceManager.Instance.LoadAsset<TextAsset>("ItemSO", eAssetType.Data);
        itemSO = JsonUtility.FromJson<ItemData>(json.text);
    }
}
