using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : SceneBase
{
    public GameObject targetObject01; // 비활성화할 오브젝트

    public float delay01 = 17f;        // 지연 시간
    public static bool fisrtPlay = false;
    private PlayerInputs playerInputs;
    public PlayerInputs.PlayerActions playerActions;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (fisrtPlay)
        {
            targetObject01.SetActive(false); // 오브젝트 비활성화
            MapManager.Instance.ShowMap<Stage01>();
            MapManager.Instance.LoadAndSpawnObjects();
            UIManager.Instance.Show<MainUI>();
            DataManager.Instance.LoadAllItems();
            return;
        }

        fisrtPlay = true;
        playerInputs.Enable();
        playerActions.Menu.performed += ActivateObject01;
        Invoke("ActivateObject02", delay01);

    }

    public void ActivateObject01(InputAction.CallbackContext context)
    {
        if (!targetObject01.activeSelf) 
        {return;}

            playerActions.Menu.performed -= ActivateObject01;

            targetObject01.SetActive(false); // 오브젝트 비활성화
            MapManager.Instance.ShowMap<Stage01>();
            MapManager.Instance.LoadAndSpawnObjects();
            UIManager.Instance.Show<MainUI>();
            DataManager.Instance.LoadAllItems();
    }
    public void ActivateObject02()
    {
        if (!targetObject01.activeSelf)
        {return;}

            playerActions.Menu.performed -= ActivateObject01;

            targetObject01.SetActive(false); // 오브젝트 비활성화
            MapManager.Instance.ShowMap<Stage01>();
            MapManager.Instance.LoadAndSpawnObjects();
            UIManager.Instance.Show<MainUI>();
            DataManager.Instance.LoadAllItems();
    }
}
