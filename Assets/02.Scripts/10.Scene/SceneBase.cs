using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        UIManager.Instance.Initalize();
        MainGameManager.Instance.paperInteractionCount = 0;
        Time.timeScale = 1f;
        if (!Main_SceneManager.Instance.isDontDestroy)
        {
            Main_SceneManager.Instance.ChangeScene("ManagerScene", () =>
            {
                Main_SceneManager.Instance.isDontDestroy = true;
                Main_SceneManager.Instance.UnLoadScene("ManagerScene");
            }, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    protected virtual void OnDestroy()
    {
    }
}
