using Newtonsoft.Json; // 수정됨: Newtonsoft.Json 사용
using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath(string fileName) => Path.Combine(Application.persistentDataPath, fileName); // 기존 유지

    /// <summary>
    /// 데이터를 JSON 형식으로 저장합니다.
    /// </summary>
    /// <typeparam name="T">저장할 데이터 타입</typeparam>
    /// <param name="data">저장할 데이터</param>
    /// <param name="fileName">저장할 파일 이름</param>
    public static void Save<T>(T data, string fileName)
    {
        if (data == null)
        {
            return;
        }

        string json = JsonConvert.SerializeObject(data, Formatting.Indented); // 수정됨: Newtonsoft.Json 사용

        try
        {
            File.WriteAllText(SavePath(fileName), json); // 기존 SavePath 메서드 활용
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data to {fileName}: {e.Message}");
        }
    }

    /// <summary>
    /// JSON 파일에서 데이터를 불러옵니다.
    /// </summary>
    /// <typeparam name="T">로드할 데이터 타입</typeparam>
    /// <param name="fileName">불러올 파일 이름</param>
    /// <returns>불러온 데이터 객체</returns>
    public static T Load<T>(string fileName)
    {
        string path = SavePath(fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"File not found: {fileName}");
            return default;
        }

        try
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json); // 수정됨: Newtonsoft.Json으로 역직렬화
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load file {fileName}. Error: {e.Message}");
            return default;
        }
    }

    /// <summary>
    /// 특정 파일을 삭제합니다.
    /// </summary>
    /// <param name="fileName">삭제할 파일 이름</param>
    public static void Delete(string fileName)
    {
        string path = SavePath(fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"File deleted: {fileName}");
        }
        else
        {
            Debug.LogWarning($"File not found: {fileName}");
        }
    }

    /// <summary>
    /// 특정 파일이 존재하는지 확인합니다.
    /// </summary>
    /// <param name="fileName">확인할 파일 이름</param>
    /// <returns>파일 존재 여부</returns>
    public static bool Exists(string fileName)
    {
        string path = SavePath(fileName);
        bool exists = File.Exists(path);
        Debug.Log($"File exists check: {fileName} - {exists}");
        return exists;
    }
}
