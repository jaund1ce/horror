using System;
using UnityEngine;

public interface ISaveable
{
    string Save(); // �����͸� JSON ���� ���ڿ��� ��ȯ
    void Load(string json); // JSON ���ڿ��� ��ü�� ������ȭ
}


[Serializable]
public class UserInfo : ISaveable
{
    public int PaperInteractionCount;
    public float Health;
    public float Stamina;
    public string Playerposition;

    public string Save() // ?? �Ⱦ��°Ű�����
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





