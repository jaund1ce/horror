using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public int paperInteractionCount;
    public Action<float> makeSound;
    public Player Player;
    public UserInfo PlayerData = new UserInfo();
    public EnemyInfo EnemyData = new EnemyInfo();
    public MapInfo MapData = new MapInfo();

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
        FindOrSetPlayer();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void MakeSound(float amount)
    {
        makeSound?.Invoke(amount);
    }
}
