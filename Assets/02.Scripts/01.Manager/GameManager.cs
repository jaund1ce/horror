using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public List<int> PaperInteraction;
    public float temMouseSensitivity = -1f;
    public bool getNewPaper;
    public bool IsHold;
    public Action<float> MakeSoundAction;
    public Player Player;
    public EnemyAI[] Enemy;
    public Component component;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindOrSetPlayer();
        getNewPaper = false;
    }


    private void FindOrSetPlayer()
    {
        Player = FindObjectOfType<Player>();
    }

    public void FindOrSetEnemy() 
    {
        Enemy = null;
        Enemy = FindObjectsOfType<EnemyAI>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PaperInteraction = new List<int>();
        FindOrSetPlayer();
        FindOrSetEnemy();
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
