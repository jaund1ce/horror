using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : mainSingleton<Main_SceneManager>
{

    public bool isDontDestroy = false; // bool ������ ���� isDontDestroy ����
                                       // true�� ���� ��, �ش� ������Ʈ�� �� ���� �ÿ��� �������� �ʵ��� �մϴ�.
                                       // �̴� ������ ���Ӽ��� �����ϱ� ���� ���˴ϴ�.

    public string NowSceneName = ""; // ���� Ȱ��ȭ�� ���� �̸��� �����ϴ� string ���� ����
                                     // �ʱⰪ�� �� ���ڿ��� �����Ǹ�, Awake���� ���� ���� �̸��� �����ɴϴ�.
                                     // �� ��ȯ �� ������ ���� �ٸ� ������ ������ �� ���˴ϴ�.                              


    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string mainSceneName = "MainScene1";
    [SerializeField] private string mainScene2Name = "MainScene2";
    [SerializeField] private string endSceneName = "EndScene";

    protected override void Awake()
    {

        base.Awake();

        NowSceneName = SceneManager.GetActiveScene().name; // SceneManager�� GetActiveScene().name�� ����Ͽ�
                                                           // ���� Ȱ��ȭ�� ���� �̸��� NowSceneName�� �����մϴ�.
                                                           // �� ������ �� ��ȯ �� �۾��� �����ϰų� �α뿡 Ȱ��˴ϴ�.
    }

    public void LoadStartScene()
    {
        ChangeScene(startSceneName);
    }

    public void LoadMainScene()
    {
        ChangeScene(mainSceneName);
        SoundManger.Instance.GetSceneSource(mainSceneName);
    }
    public void LoadMainScene2()
    {
        ChangeScene(mainScene2Name);
    }

    public void Restart()
    {
        ChangeScene(NowSceneName);
    }

    public void   LoadEndScene()
    {
            ChangeScene(endSceneName);
    }

    public void LoadScene(string SceneName)
    {
        ChangeScene(SceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EditorApplication.isPlaying = false;
#else
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Application.Quit();
#endif
    }

    public async void ChangeScene(string SceneName, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)  // string SceneName: �ε��� ���� �̸��� �����մϴ�.
                                                                                                                                 // Action callback: �� �ε� �Ϸ� �� ������ �߰� �۾��� ������ �� �ֽ��ϴ�.
                                                                                                                                 // LoadSceneMode loadSceneMode: �� �ε� ����� �����մϴ� (Single �Ǵ� Additive).
                                                                                                                                 // �� �޼���� �񵿱� ������� ���� ��ȯ�ϸ�, UI�� ���� �ε� ���¸� ǥ���ϰų� 
                                                                                                                                 // Ư�� ������ �� ��ȯ�� �Բ� ó���ϴ� �� �����մϴ�.
    {
        var op = SceneManager.LoadSceneAsync(SceneName, loadSceneMode); // SceneManager�� LoadSceneAsync�� ȣ���Ͽ�
                                                                        // �񵿱�� ���� �ε��մϴ�.
                                                                        // LoadSceneMode�� ���� ���� ���� �����ϰų� ��ü�մϴ�.


        while (!op.isDone) // op.isDone�� false�� ���� �ݺ�
        {
            await Task.Yield(); // ���� Task�� ��� ���·� �ΰ� ���� �����ӱ��� ������ ����ϴ�.
                                // �̴� ���� �����尡 ������ �ʰ� �ٸ� �۾��� ������ �� �ֵ��� �մϴ�.
        }

        if (loadSceneMode == LoadSceneMode.Single) // LoadSceneMode�� Single�̸�   
            NowSceneName = SceneName; // NowSceneName�� �ε��� ���� �̸����� ������Ʈ�մϴ�.
                                      // �̴� ���� �����尡 ������ �ʰ� �ٸ� �۾��� ������ �� �ֵ��� �մϴ�.

        callback?.Invoke(); // callback�� null�� �ƴϸ� �ش� ��������Ʈ�� �����մϴ�.
                            // �� �ε� ���� �߰� �۾��� �����ϵ��� ����Ǿ����ϴ�.
    }

    public async void UnLoadScene(string SceneName, Action callback = null)  // string SceneName: ��ε��� ���� �̸��� �����մϴ�.
                                                                             // Action callback: �� ��ε� �Ϸ� �� ������ �߰� �۾��� ������ �� �ֽ��ϴ�.
                                                                             // �� �޼���� �񵿱� ������� ���� ��ε��ϸ�, �޸� ������ ��Ȱ��ȭ�� �� ���ſ� Ȱ��˴ϴ�.

    {
        var op = SceneManager.UnloadSceneAsync(SceneName); // SceneManager�� UnloadSceneAsync�� ȣ���Ͽ�
                                                           // �񵿱�� ���� ��ε��մϴ�.
                                                           // �ε�� ���ҽ��� �����ϰ� �޸� ��뷮�� ���̴� �� �����մϴ�.

        while (!op.isDone) // op.isDone�� false�� ���� �ݺ�
        {
            await Task.Yield(); // ���� Task�� ��� ���·� �ΰ� ���� �����ӱ��� ������ ����ϴ�.
                                // �̴� ���� ������ �۾��� ��� ����� �� �ֵ��� �����մϴ�.
        }

        callback?.Invoke();  // callback�� null�� �ƴϸ� �ش� ��������Ʈ�� �����մϴ�.
                             // ��ε� �� Ư�� ������ �����ϴ� �� �����մϴ�.
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
        
}
