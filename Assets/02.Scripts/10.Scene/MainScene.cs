using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
    public GameObject targetObject01; // ��Ȱ��ȭ�� ������Ʈ

    public float delay01 = 46f;        // ���� �ð�
    public static bool fisrtPlay = false;
    // Start is called before the first frame update
    void Start()
    {
        if (fisrtPlay)
        {
            targetObject01.SetActive(false); // ������Ʈ ��Ȱ��ȭ
            MapManager.Instance.ShowMap<Stage01>();
            MapManager.Instance.LoadAndSpawnObjects();
            UIManager.Instance.Show<MainUI>();
            DataManager.Instance.LoadAllItems();
            return;
        }
        fisrtPlay = true;
        Invoke("ActivateObject01", delay01);
    }

    void ActivateObject01()
    {
        if (targetObject01 != null)
        {
            targetObject01.SetActive(false); // ������Ʈ ��Ȱ��ȭ
            MapManager.Instance.ShowMap<Stage01>();
            MapManager.Instance.LoadAndSpawnObjects();
            UIManager.Instance.Show<MainUI>();
            DataManager.Instance.LoadAllItems();
        }
    }
}
