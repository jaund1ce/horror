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

    public bool isDontDestroy = false; // bool 형식의 변수 isDontDestroy 선언
                                       // true로 설정 시, 해당 오브젝트가 씬 변경 시에도 삭제되지 않도록 합니다.
                                       // 이는 게임의 지속성을 유지하기 위해 사용됩니다.

    public string NowSceneName = ""; // 현재 활성화된 씬의 이름을 저장하는 string 형식 변수
                                     // 초기값은 빈 문자열로 설정되며, Awake에서 현재 씬의 이름을 가져옵니다.
                                     // 씬 전환 후 로직에 따라 다른 동작을 수행할 때 사용됩니다.
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


        NowSceneName = SceneManager.GetActiveScene().name; // SceneManager의 GetActiveScene().name을 사용하여
                                                           // 현재 활성화된 씬의 이름을 NowSceneName에 저장합니다.
                                                           // 이 정보는 씬 전환 후 작업을 수행하거나 로깅에 활용됩니다.
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
            // 요구사항 : 영상이 끝나고 자연스럽게 플레이랑 이어지게끔 로딩 먼저 하기 위해 코드순서 설정
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

    public async void ChangeScene(string SceneName, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)  // string SceneName: 로드할 씬의 이름을 지정합니다.
                                                                                                                                 // Action callback: 씬 로드 완료 후 실행할 추가 작업을 지정할 수 있습니다.
                                                                                                                                 // LoadSceneMode loadSceneMode: 씬 로드 방식을 지정합니다 (Single 또는 Additive).
                                                                                                                                 // 이 메서드는 비동기 방식으로 씬을 전환하며, UI를 통해 로딩 상태를 표시하거나 
                                                                                                                                 // 특정 로직을 씬 전환과 함께 처리하는 데 유용합니다.
    {

        var op = SceneManager.LoadSceneAsync(SceneName, loadSceneMode); // SceneManager의 LoadSceneAsync를 호출하여
                                                                        // 비동기로 씬을 로드합니다.
                                                                        // LoadSceneMode에 따라 기존 씬을 유지하거나 교체합니다.


        while (!op.isDone) // op.isDone이 false일 동안 반복
        {
            await Task.Yield(); // 현재 Task를 대기 상태로 두고 다음 프레임까지 실행을 멈춥니다.
                                // 이는 메인 스레드가 멈추지 않고 다른 작업을 병행할 수 있도록 합니다.
        }

        if (loadSceneMode == LoadSceneMode.Single) // LoadSceneMode가 Single이면   
            NowSceneName = SceneName; // NowSceneName을 로드한 씬의 이름으로 업데이트합니다.
                                      // 이는 메인 스레드가 멈추지 않고 다른 작업을 병행할 수 있도록 합니다.

        callback?.Invoke(); // callback이 null이 아니면 해당 델리게이트를 실행합니다.
                            // 씬 로드 이후 추가 작업을 수행하도록 설계되었습니다.
    }

    public async void UnLoadScene(string SceneName, Action callback = null)  // string SceneName: 언로드할 씬의 이름을 지정합니다.
                                                                             // Action callback: 씬 언로드 완료 후 실행할 추가 작업을 지정할 수 있습니다.
                                                                             // 이 메서드는 비동기 방식으로 씬을 언로드하며, 메모리 관리와 비활성화된 씬 제거에 활용됩니다.

    {
        var op = SceneManager.UnloadSceneAsync(SceneName); // SceneManager의 UnloadSceneAsync를 호출하여
                                                           // 비동기로 씬을 언로드합니다.
                                                           // 로드된 리소스를 해제하고 메모리 사용량을 줄이는 데 유용합니다.

        while (!op.isDone) // op.isDone이 false일 동안 반복
        {
            await Task.Yield(); // 현재 Task를 대기 상태로 두고 다음 프레임까지 실행을 멈춥니다.
                                // 이는 메인 스레드 작업이 계속 진행될 수 있도록 보장합니다.
        }

        callback?.Invoke();  // callback이 null이 아니면 해당 델리게이트를 실행합니다.
                             // 언로드 후 특정 동작을 수행하는 데 유용합니다.
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
        // 1. 로딩 씬을 Additive 모드로 로드
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadingSceneOp.isDone);

        // 2. 로딩 씬의 UI 컨트롤러 가져오기
        LoadingSceneController loadingController = FindObjectOfType<LoadingSceneController>();
        if (loadingController != null)
        {
            loadingController.SetConfirmButtonActive(false); // 버튼 비활성화
        }

        // 3. 타겟 씬을 Single 모드로 로드
        AsyncOperation targetSceneOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);
        NowSceneName = targetSceneName;

        // 4. 진행률 업데이트
        while (!targetSceneOp.isDone)
        {
            float progress = targetSceneOp.progress / 0.9f; // AsyncOperation 진행률 계산
            if (loadingController != null)
            {
                loadingController.UpdateProgress(progress); // 진행률 업데이트

            }
            yield return null;
        }
        waitSeconds = StartCoroutine(WaitCoroutine(time));
        yield return waitSeconds;

        // 6. 추가 작업 실행
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
        targetObject.SetActive(false); // 오브젝트 비활성화
    }

    public void AutoHideVideo()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Video");
        if (targetObject.activeSelf == false) return; 
        if (playerActions.Menu != null) playerActions.Menu.performed -= HideVideo;
        UIManager.Instance.Hide<SkipUI>();
        isWaitStopped = true;
        targetObject.SetActive(false); // 오브젝트 비활성화
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
