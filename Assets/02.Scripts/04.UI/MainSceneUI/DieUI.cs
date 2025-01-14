using UnityEngine;

public class DieUI : PopupUI
{
    public void LoadStart()
    {
        Main_SceneManager.Instance.LoadStartScene();
    }
    public void LoadMain()
    {
        Main_SceneManager.Instance.LoadGame();
    }
    public void LoadQuit()
    {
        Main_SceneManager.Instance.QuitGame();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Time.timeScale = 1f;
    }
    // 필요하다면 추가적인 동작 구현
    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }


}