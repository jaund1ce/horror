using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum eAssetType
{
    Prefab,       // 프리팹 자산
    Thumbnail,    // 썸네일 이미지
    UI,           // UI 관련 자산
    Data,         // 데이터 관련 파일
    SO,           // ScriptableObject
    Sound         // 사운드 파일
}

public enum eCategoryType
{
    None,         // 분류 없음
    Item,         // 아이템 자산
    NPC,          // NPC 관련 자산
    Stage,        // 스테이지 자산
    Character,    // 캐릭터 관련 자산
    Maps,         // 맵 데이터
    Model         // 3D 모델
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
