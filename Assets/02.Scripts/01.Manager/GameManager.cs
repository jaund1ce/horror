using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public int paperInteractionCount;
    public Action<float> MakeSoundAction;
    public Player Player;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindOrSetPlayer();
    }

    private void FindOrSetPlayer()
    {
        Player = FindObjectOfType<Player>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        paperInteractionCount = 0;
        FindOrSetPlayer();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void MakeSound(float amount)
    {
        MakeSoundAction?.Invoke(amount);
    }
}
