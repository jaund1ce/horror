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
            Debug.LogWarning("���� Player ������Ʈ�� �����ϴ�.");
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
