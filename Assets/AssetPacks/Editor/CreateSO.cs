using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateSO : EditorWindow
{
    public string SOpath = Path.Combine("Assets/04.Data/JsonData/ScriptableObject");//위의 방식은 이 컴퓨터의 경로를 찾는 것으로 asset의 변경은 root 내에서만 가능하기 때문에 다르게 선언해 줘야한다

    [MenuItem("JsinToSO/JsonToSO Editor")]   
    public static void CreateJsonSO()
    {
        GetWindow<CreateSO>("Data Editor");//EditorWindow의 기능
    }

    void OnGUI()//start, update 같은 정해진 방식의 기능
    {
        GUILayout.Label("데이터 도구", EditorStyles.boldLabel);
        if (GUILayout.Button("ItemSO데이터 생성 및 업데이트"))//
        {
            ChangeJsonToSO("ItemSO");
        }
        if (GUILayout.Button("PaperSO데이터 생성 및 업데이트"))
        {
            ChangeJsonToSO("PaperSO");
        }
        if (GUILayout.Button("EnemySO데이터 생성 및 업데이트"))
        {
            ChangeJsonToSO("EnemySO");
        }
        if (GUILayout.Button("PlayerSO데이터 생성 및 업데이트"))
        {
            ChangeJsonToSO("PlayerSO");
        }
    }

    public void ChangeJsonToSO(string type)
    {
        string filepath = Path.Combine(Application.dataPath, $"04.Data/JsonData/{type}data.json");//json 데이터를 넣어놓은 경로

        if (!File.Exists(filepath))
        {
            Debug.LogError("Wrong Path?");
            return;
        }

        string json = File.ReadAllText(filepath);
        if (type == "ItemSO")
        {
            ItemSO[] itemDatas = JsonHelper.FromJson<ItemSO>(json);
            if (itemDatas == null || itemDatas.Length == 0)
            {
                Debug.LogError("Wrong Json File");
                return;
            }

            foreach (var item in itemDatas)
            {
                string assetPath = $"{SOpath}/{type}/{item.ItemNameEng}.asset";

                ItemData existItemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

                if (existItemData == null)//so가 없으면
                {
                    ItemData newItemSO = ScriptableObject.CreateInstance<ItemData>();

                    newItemSO.itemSO = item;

                    AssetDatabase.CreateAsset(newItemSO, assetPath);
                }
                else//so가 이미 존재하면 업데이트 시켜줌
                {
                    existItemData.itemSO = item;
                    EditorUtility.SetDirty(existItemData);
                }
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
            string newJson = "{\"array\":" + json + "}";//json 파일을 렙핑 해줘서 사용가능한 형태로 변경. 여러 배열 앞에 이름을 붙여야 쓸 수 있는데, json에 넣어줄 수도 있지만 스크립트로 json 앞에 랩핑을 시켜줘서 사용 가능하게 만듬
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    //public void ChangeData<T>(string filepath, string type) where T : ItemData
    //{
    //    string json = File.ReadAllText(filepath);

    //    T[] Datas = JsonHelper.FromJson<T>(json);
    //    if (Datas == null || Datas.Length == 0)
    //    {
    //        Debug.LogError("Wrong Json File");
    //        return;
    //    }

    //    foreach (var item in Datas)
    //    {
    //        string assetPath = $"{SOpath}/{type}/{item.ItemNameEng}.asset";

    //        ItemSO existItemData = AssetDatabase.LoadAssetAtPath<ItemSO>(assetPath);

    //        if (existItemData == null)//so가 없으면
    //        {
    //            ItemSO newItemSO = ScriptableObject.CreateInstance<ItemSO>();

    //            newItemSO.itemData = item;

    //            AssetDatabase.CreateAsset(newItemSO, assetPath);
    //        }
    //        else//so가 이미 존재하면 업데이트 시켜줌
    //        {
    //            existItemData.itemData = item;
    //            EditorUtility.SetDirty(existItemData);
    //        }
    //    }
    //}
}
