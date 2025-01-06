using UnityEngine;

public class StartUI : BaseUI
{
    [field: SerializeField] private AudioClip buttonSound;
    private AudioSource audiosource;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }
    
    public void LoadMain()
    {
        audiosource.PlayOneShot(buttonSound);
        Main_SceneManager.Instance.LoadMainScene();
    }
    public void LoadQuit()
    {
        audiosource.PlayOneShot(buttonSound);
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
