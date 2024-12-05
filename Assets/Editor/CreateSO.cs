using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateSO : EditorWindow
{
    public string SOpath = Path.Combine("Assets/05.ScriptableObject/JsonItemSO/SO");//���� ����� �� ��ǻ���� ��θ� ã�� ������ asset�� ������ root �������� �����ϱ� ������ �ٸ��� ������ ����Ѵ�

    [MenuItem("Window/JsonToSO Editor")]   
    public static void CreateJsonSO()
    {
        GetWindow<CreateSO>("Data Editor");//EditorWindow�� ���
    }

    void OnGUI()//start, update ���� ������ ����� ���
    {
        GUILayout.Label("������ ����", EditorStyles.boldLabel);
        if (GUILayout.Button("SO������ ���� �� ������Ʈ"))//
        {
            ChangeJsonToSO();
        }
        if (GUILayout.Button("Prefab������ ����"))
        {
            
        }
    }

    public void ChangeJsonToSO()
    {
        string filepath = Path.Combine(Application.dataPath, "10.Data/data.json");//json �����͸� �־���� ���

        if (!File.Exists(filepath))
        {
            Debug.LogError("Wrong Path?");
            return;
        }

        string json = File.ReadAllText(filepath);
        ItemData[] itemDatas = JsonHelper.FromJson<ItemData>(json);

        if (itemDatas == null || itemDatas.Length == 0)
        {
            Debug.LogError("Wrong Json File");
            return;
        }

        foreach (var item in itemDatas)
        {
            string assetPath = $"{SOpath}/{item.ItemNameEng}.asset";

            ItemSO existItemData = AssetDatabase.LoadAssetAtPath<ItemSO>(assetPath);

            if (existItemData == null)//so�� ������
            {
                ItemSO newItemSO = ScriptableObject.CreateInstance<ItemSO>();
                
                newItemSO.itemData = item;

                AssetDatabase.CreateAsset(newItemSO, assetPath);
            }
            else//so�� �̹� �����ϸ� ������Ʈ ������
            {
                existItemData.itemData = item;
                EditorUtility.SetDirty(existItemData);
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
}
