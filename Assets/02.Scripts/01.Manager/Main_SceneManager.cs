using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : mainSingleton<Main_SceneManager>
{

    public bool isDontDestroy = false; // bool 형식의 변수 isDontDestroy 선언
                                       // true로 설정 시, 해당 오브젝트가 씬 변경 시에도 삭제되지 않도록 합니다.
                                       // 이는 게임의 지속성을 유지하기 위해 사용됩니다.

    public string NowSceneName = ""; // 현재 활성화된 씬의 이름을 저장하는 string 형식 변수
                                     // 초기값은 빈 문자열로 설정되며, Awake에서 현재 씬의 이름을 가져옵니다.
                                     // 씬 전환 후 로직에 따라 다른 동작을 수행할 때 사용됩니다.
    public string PreviousSceneName = "";


    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string mainSceneName = "MainScene";
    [SerializeField] private string mainScene2Name = "MainScene2";
    [SerializeField] private string endSceneName = "EndScene";
    [SerializeField] private string loadingSceneName = "LoadingScene";

    protected override void Awake()
    {

        base.Awake();

        NowSceneName = SceneManager.GetActiveScene().name; // SceneManager의 GetActiveScene().name을 사용하여
                                                           // 현재 활성화된 씬의 이름을 NowSceneName에 저장합니다.
                                                           // 이 정보는 씬 전환 후 작업을 수행하거나 로깅에 활용됩니다.
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
            PreviousSceneName = NowSceneName;
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

    public void LoadLoadingScene(string targetSceneName, Action callback = null)
    {
        StartCoroutine(LoadSceneWithControl(targetSceneName, callback));
    }

    private IEnumerator LoadSceneWithControl(string targetSceneName, Action callback)
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

        // 5. 로딩 씬을 언로드
        //AsyncOperation unloadSceneOp = SceneManager.UnloadSceneAsync(loadingSceneName);
        //yield return new WaitUntil(() => unloadSceneOp.isDone);

        // 6. 추가 작업 실행
        callback?.Invoke();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
        
}
