using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UHFPS.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class JsonToSO : MonoBehaviour
{
    //public string SOpath = Path.Combine(Application.dataPath, "05.ScriptableObject/JsonItemSO/SO");->c/\ user.... 처럼 나오기 때문에
    public string SOpath = Path.Combine("Assets/05.ScriptableObject/JsonItemSO/SO");//위의 방식은 이 컴퓨터의 경로를 찾는 것으로 asset의 변경은 root 내에서만 가능하기 때문에 다르게 선언해 줘야한다

    private void Start()
    {
        ChangeJsonToSO();//
    }

    public void ChangeJsonToSO()
    {
        string filepath = Path.Combine(Application.dataPath, "10.Data/data.json");

        if (!File.Exists(filepath)) 
        {
            Debug.LogError("Wrong Path?");
            return;
        }

        string json = File.ReadAllText(filepath);
        JsonItemData[] itemDatas = JsonHelper.FromJson<JsonItemData>(json);

        if(itemDatas == null || itemDatas.Length == 0)
        {
            Debug.LogError("Wrong Json File");
            return;
        }

        foreach (var item in itemDatas)
        {
            string assetPath = $"{SOpath}{item.ItemNameEng}.asset";

            ItemSO existItemData = AssetDatabase.LoadAssetAtPath<ItemSO>(assetPath);

            if (existItemData == null)//so가 없으면
            {
                ItemSO newItemSO = ScriptableObject.CreateInstance<ItemSO>();
                newItemSO.itemData = item;

                
                AssetDatabase.CreateAsset(newItemSO, assetPath);
            }
            else//so가 이미 존재하면 업데이트 시켜줌
            {
                existItemData.itemData = item;
                EditorUtility.SetDirty(existItemData);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject 생성");
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"array\":"+ json +"}";//json 파일을 렙핑 해줘서 사용가능한 형태로 변경. 여러 배열 앞에 이름을 붙여야 쓸 수 있는데, json에 넣어줄 수도 있지만 스크립트로 json 앞에 랩핑을 시켜줘서 사용 가능하게 만듬
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
