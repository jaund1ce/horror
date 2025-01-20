using UnityEditor;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Drawing.Text;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Main_SceneManager : mainSingleton<Main_SceneManager>
{

    public bool isDontDestroy = false; // bool ������ ���� isDontDestroy ����
                                       // true�� ���� ��, �ش� ������Ʈ�� �� ���� �ÿ��� �������� �ʵ��� �մϴ�.
                                       // �̴� ������ ���Ӽ��� �����ϱ� ���� ���˴ϴ�.

    public string NowSceneName = ""; // ���� Ȱ��ȭ�� ���� �̸��� �����ϴ� string ���� ����
                                     // �ʱⰪ�� �� ���ڿ��� �����Ǹ�, Awake���� ���� ���� �̸��� �����ɴϴ�.
                                     // �� ��ȯ �� ������ ���� �ٸ� ������ ������ �� ���˴ϴ�.
                                     // 


    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string mainSceneName = "MainScene1";
    [SerializeField] private string endSceneName = "EndScene";
    [SerializeField] private string loadingSceneName = "LoadingScene";

    PlayerInputs playerInputs;
    PlayerInputs.PlayerActions playerActions;
    private Coroutine waitSeconds;
    private bool isWaitStopped;
    private float videoPlayTime = 17f;

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
        SoundManger.Instance.GetSceneSource(startSceneName);
    }


    public void NewGame()
    {
        LoadLoadingScene(mainSceneName, videoPlayTime, NewGameInitalize);
        SoundManger.Instance.GetSceneSource(mainSceneName);
    }
    public void LoadGame() 
    {
        if (DataManager.Instance.MapData.SceneName == null)
        {
            LoadLoadingScene(mainSceneName, videoPlayTime, NewGameInitalize);
            SoundManger.Instance.GetSceneSource(mainSceneName);
        }
        else 
        {
            // �䱸���� : ������ ������ �ڿ������� �÷��̶� �̾����Բ� �ε� ���� �ϱ� ���� �ڵ���� ����
            LoadLoadingScene(DataManager.Instance.MapData.SceneName, 0f, LoadGameInitalize);
            SoundManger.Instance.GetSceneSource(SceneManager.GetActiveScene().name);
        }
    }

    public void Restart()
    {
        ChangeScene(SceneManager.GetActiveScene().name);
        SoundManger.Instance.GetSceneSource(SceneManager.GetActiveScene().name);
    }

    public void LoadEndScene()
    {
        ChangeScene(endSceneName);
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

    public void LoadLoadingScene(string targetSceneName,float time, Action callback = null)
    {
        if (callback.Method.Name == "NewGameInitalize") 
        {
            IntroControl();
        }
        StartCoroutine(LoadSceneWithControl(targetSceneName, time,  callback));
    }

    private IEnumerator LoadSceneWithControl(string targetSceneName,float time,  Action callback)
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
        waitSeconds = StartCoroutine(WaitCoroutine(time));
        yield return waitSeconds;

        // 6. �߰� �۾� ����
        callback?.Invoke();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void NewGameInitalize() 
    {
        MapManager.Instance.ShowMap<Stage01>();
        DataManager.Instance.LoadAllItems();
        MapManager.Instance.LoadAndSpawnObjects(1);
        MapManager.Instance.LoadAndSpawnPapers(1);
        UIManager.Instance.Show<MainUI>();
        AutoHideVideo();
    }

    private void LoadGameInitalize()
    {
        string loadMap = DataManager.Instance.MapData.MapName;
        switch(loadMap)
        { 
            case "Stage01":
                MapManager.Instance.ShowMap<Stage01>();
                break;
            case "Stage02":
                MapManager.Instance.ShowMap<Stage02>();
                break;
        }
        DataManager.Instance.LoadAllItems();
        DataManager.Instance.LoadGame();
        UIManager.Instance.Show<MainUI>();
        if(loadMap == "Stage01") AutoHideVideo();
    }

    public void IntroControl()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;
        playerInputs.Enable();
        //UIManager.Instance.Show<SkipUI>(); 
        playerActions.Menu.performed += HideVideo;

    }

    public void HideVideo(InputAction.CallbackContext context)
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Video");
        if (targetObject.activeSelf == false)
        { return; }
        playerActions.Menu.performed -= HideVideo;
        playerInputs.Disable();
        UIManager.Instance.Hide<SkipUI>();
        isWaitStopped = true;
        targetObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
    }

    public void AutoHideVideo()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Video");
        if (targetObject.activeSelf == false) return; 
        if (playerActions.Menu != null) playerActions.Menu.performed -= HideVideo;
        UIManager.Instance.Hide<SkipUI>();
        isWaitStopped = true;
        targetObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
    }

    private IEnumerator WaitCoroutine(float time) 
    {
        string loadMap = DataManager.Instance.MapData.MapName;
        if (loadMap == "Stage01") 
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag("Video");
            targetObject.SetActive(true);
            UIManager.Instance.Show<SkipUI>();
            isWaitStopped = false;
        }

        float checkTime = 0f;

        while (checkTime < time)
        {
            if (isWaitStopped)
            {
                yield break;
            }

            checkTime += Time.deltaTime;
            yield return null;
        }
    }
}
