using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : mainSingleton<MainGameManager>
{
    public int paperInteractionCount;
    public bool getNewPaper;
    public Action<float> MakeSoundAction;
    public Player Player;
    [SerializeField]public GameObject targetObject;
    [SerializeField]public string componentName;
    public Component component;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindOrSetPlayer();
        getNewPaper = false;
    }

    protected override void Start()
    {
        Type type = Type.GetType(componentName);
        if (type != null)
        {
            component = targetObject.GetComponent(type);
        }
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
