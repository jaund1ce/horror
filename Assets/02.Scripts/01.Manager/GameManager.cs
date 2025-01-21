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
    public Action<GameObject> OnObjectDestroyedAction;
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

    public void InitalizePaperData() 
    {
        PaperInteraction = new List<int>();
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

    public void NotifyObjectDestroyed(GameObject destroyedObject)
    {
        // 파괴된 오브젝트에 대한 처리를 다른 구독자에게 알림
        OnObjectDestroyedAction?.Invoke(destroyedObject);
    }
}
