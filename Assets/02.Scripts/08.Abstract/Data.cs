using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;

public interface ISaveable
{
    string Save(); // 데이터를 JSON 형식 문자열로 변환
    void Load(string json); // JSON 문자열을 객체로 역직렬화
}


[System.Serializable]
public class UserInfo : ISaveable
{
    public int paperInteractionCount;
    public InventoryData[] InventoryDatas = new InventoryData[15];
    public InventoryData[] QuickSlotDatas = new InventoryData[5];

    public string Save()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void Load(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}

[System.Serializable]
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

[System.Serializable]
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





