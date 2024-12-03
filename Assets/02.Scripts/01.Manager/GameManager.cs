using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public UserInfo User;
    public Player Player;

    protected override void Start()
    {

        SaveUserData();
        Player = FindObjectOfType<Player>();
    }

    public void SaveUserData()
    {
        string data = JsonUtility.ToJson(User, true);

        string path = Path.Combine(Application.dataPath, User.Name + ".json");

        File.WriteAllText(path, data);

        Debug.Log("저장 완료 : " + path);
    }

    public void LoadUserData(string userName)
    {
        string path = Path.Combine(Application.dataPath, userName + ".json");

        string data = File.ReadAllText(path);

        User = JsonUtility.FromJson<UserInfo>(data);

        Debug.Log("로드 완료 : " + path);
    }
}
