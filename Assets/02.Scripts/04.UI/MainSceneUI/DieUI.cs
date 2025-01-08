using UnityEngine;

public class DieUI : PopupUI
{
    public void LoadStart()
    {
        Main_SceneManager.Instance.LoadStartScene();
    }
    public void LoadMain()
    {
        Main_SceneManager.Instance.Restart();
    }
    public void LoadQuit()
    {
        Main_SceneManager.Instance.QuitGame();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
        SoundManger.Instance.ChangeBGMSound(4);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Time.timeScale = 1f;
        SoundManger.Instance.ResetAllSounds();
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