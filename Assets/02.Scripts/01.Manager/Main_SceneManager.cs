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
    public string PreviousSceneName = "";


    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string mainSceneName = "MainScene";
    [SerializeField] private string mainScene2Name = "MainScene2";
    [SerializeField] private string endSceneName = "EndScene";
    [SerializeField] private string loadingSceneName = "LoadingScene";

    protected override void Awake()
    {

        base.Awake();

        NowSceneName = SceneManager.GetActiveScene().name; // SceneManager�� GetActiveScene().name�� ����Ͽ�
                                                           // ���� Ȱ��ȭ�� ���� �̸��� NowSceneName�� �����մϴ�.
                                                           // �� ������ �� ��ȯ �� �۾��� �����ϰų� �α뿡 Ȱ��˴ϴ�.
    }

    public void LoadStartScene()
    {
        SoundManger.Instance.GetSceneSource(startSceneName);
        ChangeScene(startSceneName);
    }

    public void LoadMainScene()
    {
        LoadLoadingScene(mainSceneName);
        SoundManger.Instance.GetSceneSource(mainSceneName);
        SoundManger.Instance.GetSceneSource(mainSceneName);
    }
    public void LoadMainScene2()
    {
        SoundManger.Instance.GetSceneSource(mainScene2Name);
        LoadLoadingScene(mainScene2Name);
    }

    public void Restart()
    {
        SoundManger.Instance.GetSceneSource(SceneManager.GetActiveScene().name);
        ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void LoadEndScene()
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
            PreviousSceneName = NowSceneName;
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

    public void LoadLoadingScene(string targetSceneName, Action callback = null)
    {
        StartCoroutine(LoadSceneWithControl(targetSceneName, callback));
    }

    private IEnumerator LoadSceneWithControl(string targetSceneName, Action callback)
    {
        // 1. �ε� ���� Additive ���� �ε�
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadingSceneOp.isDone);

        // 2. �ε� ���� UI ��Ʈ�ѷ� ��������
        LoadingSceneController loadingController = FindObjectOfType<LoadingSceneController>();
        if (loadingController != null)
        {
            loadingController.SetConfirmButtonActive(false); // ��ư ��Ȱ��ȭ
        }

        // 3. Ÿ�� ���� Single ���� �ε�
        AsyncOperation targetSceneOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);
        NowSceneName = targetSceneName;

        // 4. ����� ������Ʈ
        while (!targetSceneOp.isDone)
        {
            float progress = targetSceneOp.progress / 0.9f; // AsyncOperation ����� ���
            if (loadingController != null)
            {
                loadingController.UpdateProgress(progress); // ����� ������Ʈ

            }
            yield return null;
        }

        // 5. �ε� ���� ��ε�
        //AsyncOperation unloadSceneOp = SceneManager.UnloadSceneAsync(loadingSceneName);
        //yield return new WaitUntil(() => unloadSceneOp.isDone);

        // 6. �߰� �۾� ����
        callback?.Invoke();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
        
}
