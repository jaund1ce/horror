using UnityEngine;

public class StartUI : BaseUI
{
    public void NewGameBtn()
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        Main_SceneManager.Instance.NewGame();
    }
    public void LoadGameBtn() 
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        Main_SceneManager.Instance.LoadGame();
    }

    public void QuitBtn()
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
