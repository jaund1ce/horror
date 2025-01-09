using UnityEngine;

public class SystemUI : PopupUI
{
    public void LoadStart()
    {
        Main_SceneManager.Instance.LoadStartScene();
    }
    public void LoadMain()
    {
        Main_SceneManager.Instance.Restart();
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

    public void Save()
    {
        DataManager.Instance.SaveGame(true);
    }

    public void Load()
    {
        DataManager.Instance.LoadGame();
    }


}