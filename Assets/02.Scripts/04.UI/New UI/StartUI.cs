using UnityEngine;

public class StartUI : BaseUI
{
    public void LoadMain()
    {
        Main_SceneManager.Instance.LoadMainScene();
    }
    public void LoadQuit()
    {
        Main_SceneManager.Instance.QuitGame();
    }
    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }
}
