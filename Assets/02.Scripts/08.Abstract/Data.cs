using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string Save(); // �����͸� JSON ���� ���ڿ��� ��ȯ
    void Load(string json); // JSON ���ڿ��� ��ü�� ������ȭ
}


[Serializable]
public class UserInfo : ISaveable
{
    public string PlayerName = "Player_noneParkourtic2";
    public List<int> PaperInteraction;
    public float Health;
    public float Stamina;
    public string Playerposition;

    public string Save() // ?? �Ⱦ��°Ű�����
    {
        PaperInteraction = MainGameManager.Instance.PaperInteraction;
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
        MainGameManager.Instance.PaperInteraction = PaperInteraction;
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
    public string SceneName;
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





