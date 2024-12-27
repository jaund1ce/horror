using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public int paperInteractionCount;
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
        if (Player == null)
        {
            Debug.LogWarning("씬에 Player 오브젝트가 없습니다.");
        }
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
}
