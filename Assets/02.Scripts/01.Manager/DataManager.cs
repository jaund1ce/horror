using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : mainSingleton<DataManager>
{
   private ItemData itemSO;
   public List<ItemSO> AllItems = new List<ItemSO>();

    public ItemData ItemSO
   {

    get { 
            if(itemSO == null)
            {
                LoadData();
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

   

    //private void GetData(string Kor_name)
    //{
    //  var item = ItemDataList.utemData.Find(x => x.id == Kor_name);
    //  return item;
    //}

    private void LoadData()
    {
        var json = ResourceManager.Instance.LoadAsset<TextAsset>("ItemSO", eAssetType.Data); // Resources/Data/ItemSo파일을 자산으로 활용하기 위해 assetPool에 등록 후 지역변수 json에 TextAsset자료형으로 할당
        itemSO = JsonUtility.FromJson<ItemData>(json.text); //TextAsset에서 JSON 문자열 데이터 추출하여 JSON 데이터를 ItemData로 변환하여 itemSO에 저장
    }
}
