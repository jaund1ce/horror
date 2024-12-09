
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_SceneManager : MonoBehaviour
{
    [SerializeField] private string startSceneName;
    [SerializeField] private string mainSceneName;
    [SerializeField] private string endSceneName;

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

    public void LoadEndScene()
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
        EditorApplication.isPlaying = false; // 에디터 실행 중단
        #else
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Application.Quit();
        #endif
    }
}
