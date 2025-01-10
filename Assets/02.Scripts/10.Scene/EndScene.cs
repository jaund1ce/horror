using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndScene : SceneBase
{
    public GameObject targetObject; // 비활성화할 오브젝트
    private PlayerInputs playerInputs;
    public PlayerInputs.PlayerActions playerActions;

    public float delay = 26f;        // 지연 시간
    public float delay2 = 18f;       // 음악 재생 지연 시간
    // Start is called before the first frame update
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;
    }

    void Start()
    {
        playerInputs.Enable();
        playerActions.Menu.performed += ActivateObject;
        Invoke("ActivateObject", delay);
        Invoke("ActivateSound", delay2);
    }


    void ActivateObject(InputAction.CallbackContext context)
    {
        if (!this.gameObject.activeSelf)
        { return; }
        playerActions.Menu.performed -= ActivateObject;
        playerInputs.Disable();
        if (targetObject != null)
        {
            SoundManger.Instance.ChangeBGMSound(4);
            targetObject.SetActive(false); // 오브젝트 비활성화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.Show<EndUI>();
        }
    }

    void ActivateSound()
    {
        if(!this.gameObject.activeSelf) 
        { return; }
        SoundManger.Instance.ChangeBGMSound(4);
    }

    void ResetSound()
    {
        SoundManger.Instance.ResetAllSounds();
    }
}
