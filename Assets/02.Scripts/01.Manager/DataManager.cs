using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
   private ItemSO itemSO;
    
   public ItemSO ItemSO
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
    }

    private void GetData()
    {
        var json = ResourceManager.Instance.LoadAsset<TextAsset>("ItemSO", eAssetType.Data);
        itemSO = JsonUtility.FromJson<ItemSO>(json.text);
    }
}
