using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float itemCheckDistance;
    [SerializeField] private float itemCheckTime = 0.1f;

    private float lastCheckTime;
    [SerializeField]private LayerMask iteractableLayerMask;
    
    public PlayerInputs playerInputs { get; private set; }//inputsystem generate c# script�� ������ ��ũ��Ʈ
    public PlayerInputs.PlayerActions playerActions { get; private set; }   //�̸� ������ �ൿ�� move, look,... ��

    public IInteractable CurrentInteracteable;
    private bool isPuzzle;
    private bool PuzzleEnter;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;//inputsystem�� �����ߴ� Actionmap �߿� �ϳ��� ����
        playerInputs.Enable();
        //playerInputs.Player.Look.performed += _ => getItemData(); //look�� ���콺 ��Ÿ ���� �ޱ� ������ X
    }

    private void Update()
    {
        if (Time.time - lastCheckTime < itemCheckTime) return;

        lastCheckTime = Time.time;
        getItemData();   
    }

    private void getItemData()//���� �ٶ󺸴� ������ ǥ��
    {
        Vector3 sceenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(sceenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, itemCheckDistance, iteractableLayerMask))//��� iteractable layer�� iinteractable�� ������ �ִ�.
        {
            IInteractable iteractable = hit.collider.GetComponent<IInteractable>();
            if (iteractable as PuzzleBase ) isPuzzle = true;
            else isPuzzle = false;
            if (iteractable == null) return;
            else if (iteractable == CurrentInteracteable) return;
            CurrentInteracteable = iteractable;
        }
        else CurrentInteracteable = null;

        if (!PuzzleEnter)
        {
            MainUI mainUI = UIManager.Instance.GetUI<MainUI>();
            mainUI.ShowPromptUI(CurrentInteracteable);
        }
    }

    private void handleInteractionInput(InputAction.CallbackContext context)//��ȣ�ۿ�� ������ ȸ��
    {
        if (CurrentInteracteable == null) return;
        HandleInputAndPrompt();
        CurrentInteracteable.OnInteract();
    }

    public void HandleInputAndPrompt() 
    {
        MainUI mainUI = UIManager.Instance.GetUI<MainUI>();

        if (isPuzzle && !PuzzleEnter)
        {
            PuzzleEnter = true;
            MainGameManager.Instance.Player.Input.InputUnsubscribe();
            mainUI.ShowPromptUI(null);
            return;
        }
        else if (PuzzleEnter)
        {
            PuzzleEnter = false;
            MainGameManager.Instance.Player.Input.InputSubscribe();
        }
            mainUI.ShowPromptUI(CurrentInteracteable);
        
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerActions.Interaction.started += handleInteractionInput;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerActions.Interaction.started -= handleInteractionInput;
    }
}
