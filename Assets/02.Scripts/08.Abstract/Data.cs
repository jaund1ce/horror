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
    public int paperInteractionCount;
    public InventoryData[] InventoryDatas = new InventoryData[15];
    public InventoryData[] QuickSlotDatas = new InventoryData[4];

    public string Save()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}

[Serializable]
public class EnemyInfo : ISaveable
{
    public string EnemyType;
    public int PositionX, PositionY, PositionZ;

    public string Save()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}

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





