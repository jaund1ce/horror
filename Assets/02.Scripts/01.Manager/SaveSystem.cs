using Newtonsoft.Json; // ������: Newtonsoft.Json ���
using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath(string fileName) => Path.Combine(Application.persistentDataPath, fileName); // ���� ����

    /// <summary>
    /// �����͸� JSON �������� �����մϴ�.
    /// </summary>
    /// <typeparam name="T">������ ������ Ÿ��</typeparam>
    /// <param name="data">������ ������</param>
    /// <param name="fileName">������ ���� �̸�</param>
    public static void Save<T>(T data, string fileName)
    {
        if (data == null)
        {
            Debug.LogError($"Attempting to save null data to {fileName}"); // ���� ����
            return;
        }

        string json = JsonConvert.SerializeObject(data, Formatting.Indented); // ������: Newtonsoft.Json ���
        Debug.Log($"Saving data to {fileName}: {json}");

        try
        {
            File.WriteAllText(SavePath(fileName), json); // ���� SavePath �޼��� Ȱ��
            Debug.Log($"Save successful: {fileName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data to {fileName}: {e.Message}");
        }
    }

    /// <summary>
    /// JSON ���Ͽ��� �����͸� �ҷ��ɴϴ�.
    /// </summary>
    /// <typeparam name="T">�ε��� ������ Ÿ��</typeparam>
    /// <param name="fileName">�ҷ��� ���� �̸�</param>
    /// <returns>�ҷ��� ������ ��ü</returns>
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
            Debug.Log($"Loading data from {fileName}: {json}"); // ������: JSON ����� �߰�
            return JsonConvert.DeserializeObject<T>(json); // ������: Newtonsoft.Json���� ������ȭ
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load file {fileName}. Error: {e.Message}");
            return default;
        }
    }

    /// <summary>
    /// Ư�� ������ �����մϴ�.
    /// </summary>
    /// <param name="fileName">������ ���� �̸�</param>
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
    /// Ư�� ������ �����ϴ��� Ȯ���մϴ�.
    /// </summary>
    /// <param name="fileName">Ȯ���� ���� �̸�</param>
    /// <returns>���� ���� ����</returns>
    public static bool Exists(string fileName)
    {
        string path = SavePath(fileName);
        bool exists = File.Exists(path);
        Debug.Log($"File exists check: {fileName} - {exists}");
        return exists;
    }
}
