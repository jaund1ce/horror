using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum eAssetType
{
    Prefab,       // ������ �ڻ�
    Thumbnail,    // ����� �̹���
    UI,           // UI ���� �ڻ�
    Data,         // ������ ���� ����
    SO,           // ScriptableObject
    Sound         // ���� ����
}

public enum eCategoryType
{
    None,         // �з� ����
    Item,         // ������ �ڻ�
    NPC,          // NPC ���� �ڻ�
    Stage,        // �������� �ڻ�
    Character,    // ĳ���� ���� �ڻ�
    Maps,         // �� ������
    Model         // 3D ��
}

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, object> assetPool = new Dictionary<string, object>();

    public T LoadAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None)
    {
        T handle = default;

        var typeStr = $"{assetType}{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}";

        if (!assetPool.ContainsKey(key + "_" + typeStr))
        {
            var obj = Resources.Load($"{typeStr}/{key}", typeof(T));

            if (obj == null)
                return default;

            assetPool.Add(key + "_" + typeStr, obj);
        }

        handle = (T)assetPool[key + "_" + typeStr];

        return handle;
    }

    public async Task<T> LoadAsyncAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None)
    {
        T handle = default;

        var typeStr = $"{assetType}{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}";

        if (!assetPool.ContainsKey(key + "_" + typeStr))
        {
            var op = Resources.LoadAsync($"{typeStr}/{key}", typeof(T));

            while (!op.isDone)
            {
                await Task.Yield();
            }

            var obj = op.asset;

            if (obj == null)
                return default;

            assetPool.Add(key + "_" + typeStr, obj);
        }

        handle = (T)assetPool[key + "_" + typeStr];

        return handle;
    }
}
