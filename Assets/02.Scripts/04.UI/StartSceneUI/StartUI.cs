using UnityEngine;

public class StartUI : BaseUI
{
    public void NewGameBtn()
    {
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        Main_SceneManager.Instance.NewGame();
    }
    public void LoadGameBtn() 
    {
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        Main_SceneManager.Instance.LoadGame();
    }

    public void QuitBtn()
    {
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        Main_SceneManager.Instance.QuitGame();
    }
    public override void OpenUI()
    {
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        base.OpenUI();
    }

    public override void CloseUI()
    {
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        base.CloseUI();
    }
}
