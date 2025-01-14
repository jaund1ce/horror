using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndScene : SceneBase
{
    public GameObject targetObject; 
    private PlayerInputs playerInputs;
    public PlayerInputs.PlayerActions playerActions;

    public float delay = 26f;       
    public float delay2 = 18f;     
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
        UIManager.Instance.Show<SkipUI>();
        Invoke("ActivateSound", delay2);
        Invoke("ActivateObject2", delay);
    }


    void ActivateObject(InputAction.CallbackContext context)
    {
        if (targetObject.activeSelf == false)
        { return; }
        UIManager.Instance.Hide<SkipUI>();
        playerActions.Menu.performed -= ActivateObject;
        playerInputs.Disable();
        if (targetObject != null)
        {
            SoundManger.Instance.ChangeBGMSound(4);
            targetObject.SetActive(false); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.Show<EndUI>();
        }
    }

    void ActivateObject2()
    {
        if (targetObject.activeSelf == false)
        { return; }
        playerActions.Menu.performed -= ActivateObject;
        playerInputs.Disable();
        if (targetObject != null)
        {
            targetObject.SetActive(false); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.Show<EndUI>();
        }
    }

    void ActivateSound()
    {
        if (targetObject.activeSelf == false)
        { return; }
        SoundManger.Instance.ChangeBGMSound(4);
    }

    void ResetSound()
    {
        SoundManger.Instance.ResetAllSounds();
    }
}