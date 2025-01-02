using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateSO : EditorWindow
{
    public string SOpath = Path.Combine("Assets/04.Data/JsonData/ScriptableObject");//���� ����� �� ��ǻ���� ��θ� ã�� ������ asset�� ������ root �������� �����ϱ� ������ �ٸ��� ������ ����Ѵ�

    [MenuItem("JsinToSO/JsonToSO Editor")]   
    public static void CreateJsonSO()
    {
        GetWindow<CreateSO>("Data Editor");//EditorWindow�� ���
    }

    void OnGUI()//start, update ���� ������ ����� ���
    {
        GUILayout.Label("������ ����", EditorStyles.boldLabel);
        if (GUILayout.Button("ItemSO������ ���� �� ������Ʈ"))//
        {
            ChangeJsonToSO("ItemSO");
        }
        if (GUILayout.Button("PaperSO������ ���� �� ������Ʈ"))
        {
            ChangeJsonToSO("PaperSO");
        }
        if (GUILayout.Button("EnemySO������ ���� �� ������Ʈ"))
        {
            ChangeJsonToSO("EnemySO");
        }
        if (GUILayout.Button("PlayerSO������ ���� �� ������Ʈ"))
        {
            ChangeJsonToSO("PlayerSO");
        }
    }

    public void ChangeJsonToSO(string type)
    {
        string filepath = Path.Combine(Application.dataPath, $"04.Data/JsonData/{type}data.json");//json �����͸� �־���� ���

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

                if (existItemData == null)//so�� ������
                {
                    ItemData newItemSO = ScriptableObject.CreateInstance<ItemData>();

                    newItemSO.itemSO = item;

                    AssetDatabase.CreateAsset(newItemSO, assetPath);
                }
                else//so�� �̹� �����ϸ� ������Ʈ ������
                {
                    existItemData.itemSO = item;
                    EditorUtility.SetDirty(existItemData);
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject ����");
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";//json ������ ���� ���༭ ��밡���� ���·� ����. ���� �迭 �տ� �̸��� �ٿ��� �� �� �ִµ�, json�� �־��� ���� ������ ��ũ��Ʈ�� json �տ� ������ �����༭ ��� �����ϰ� ����
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

    //        if (existItemData == null)//so�� ������
    //        {
    //            ItemSO newItemSO = ScriptableObject.CreateInstance<ItemSO>();

    //            newItemSO.itemData = item;

    //            AssetDatabase.CreateAsset(newItemSO, assetPath);
    //        }
    //        else//so�� �̹� �����ϸ� ������Ʈ ������
    //        {
    //            existItemData.itemData = item;
    //            EditorUtility.SetDirty(existItemData);
    //        }
    //    }
    //}
}
