using UnityEngine;

public class StartUI : BaseUI
{
    public void LoadMain()
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        Main_SceneManager.Instance.LoadMainScene();
    }
    public void LoadQuit()
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        Main_SceneManager.Instance.QuitGame();
    }
    public override void OpenUI()
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        base.OpenUI();
    }

    public override void CloseUI()
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        base.CloseUI();
    }
}
