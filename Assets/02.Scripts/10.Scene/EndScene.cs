using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndScene : SceneBase
{
    public GameObject targetObject; // ��Ȱ��ȭ�� ������Ʈ
    private PlayerInputs playerInputs;
    public PlayerInputs.PlayerActions playerActions;

    public float delay = 26f;        // ���� �ð�
    public float delay2 = 18f;       // ���� ��� ���� �ð�
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
            targetObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
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
