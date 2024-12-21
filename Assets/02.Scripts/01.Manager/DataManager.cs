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
        var json = ResourceManager.Instance.LoadAsset<TextAsset>("ItemSO", eAssetType.Data); // Resources/Data/ItemSo������ �ڻ����� Ȱ���ϱ� ���� assetPool�� ��� �� �������� json�� TextAsset�ڷ������� �Ҵ�
        itemSO = JsonUtility.FromJson<ItemData>(json.text); //TextAsset���� JSON ���ڿ� ������ �����Ͽ� JSON �����͸� ItemData�� ��ȯ�Ͽ� itemSO�� ����
    }
}
