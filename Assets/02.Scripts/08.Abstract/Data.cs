using System;
using UnityEngine;

public interface ISaveable
{
    string Save(); // 데이터를 JSON 형식 문자열로 변환
    void Load(string json); // JSON 문자열을 객체로 역직렬화
}


[Serializable]
public class UserInfo : ISaveable
{
    public int PaperInteractionCount;
    public float Health;
    public float Stamina;
    public string Playerposition;

    public string Save() // ?? 안쓰는거같은데
    {
        PaperInteractionCount = MainGameManager.Instance.paperInteractionCount;
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
        MainGameManager.Instance.paperInteractionCount = PaperInteractionCount;
    }
}

/*[Serializable]
public class EnemyInfo : ISaveable
{
    public string EnemyType;
    public string EnemyPosition;
    public string AssetType;
    public string CategoryType;

    public string Save()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}*/

[Serializable]
public class MapInfo : ISaveable
{
    public string MapName;
    public int[] ExploredAreas;

    public string Save()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}





