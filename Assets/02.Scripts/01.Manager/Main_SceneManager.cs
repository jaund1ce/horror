using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : mainSingleton<Main_SceneManager>
{
    public bool isDontDestroy = false;

    public string NowSceneName = "";


    [SerializeField] private string startSceneName;
    [SerializeField] private string mainSceneName;
    [SerializeField] private string endSceneName;

    protected override void Awake()
    {

        Create();

        if (_instance != this)
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }
        DontDestroyOnLoad(this);

    }

    public void LoadStartScene()
    {
        if (!string.IsNullOrEmpty(startSceneName))
        {
            SceneManager.LoadScene(startSceneName);
        }
    }

    public void LoadMainScene()
    {
        if (!string.IsNullOrEmpty(mainSceneName))
        {
            SceneManager.LoadScene(mainSceneName);
        }
    }

    public void   LoadEndScene()
    {
        if (!string.IsNullOrEmpty(endSceneName))
        {
            SceneManager.LoadScene(endSceneName);
        }
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

    public async void ChangeScene(string SceneName, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var op = SceneManager.LoadSceneAsync(SceneName, loadSceneMode);

        while (!op.isDone)
        {
            Debug.Log("로딩창 띄우기");
            await Task.Yield();
        }

        if (loadSceneMode == LoadSceneMode.Single)
            NowSceneName = SceneName;

        callback?.Invoke();
    }

    public async void UnLoadScene(string SceneName, Action callback = null)
    {
        var op = SceneManager.UnloadSceneAsync(SceneName);

        while (!op.isDone)
        {
            await Task.Yield();
        }

        callback?.Invoke();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
        
}
