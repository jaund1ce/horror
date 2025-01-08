using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public int paperInteractionCount;
    public int fisrtStart;
    public Action<float> MakeSoundAction;
    public Player Player;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindOrSetPlayer();
        paperInteractionCount = 0;
        fisrtStart = 0;
    }

    private void FindOrSetPlayer()
    {
        Player = FindObjectOfType<Player>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
