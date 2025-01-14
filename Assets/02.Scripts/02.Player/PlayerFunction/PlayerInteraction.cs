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
    private bool puzzleAccess;

    public MainUI mainUI;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if(mainUI == null)
        {
            mainUI = UIManager.Instance.GetUI<MainUI>();
        }

        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;
        playerInputs.Enable();
    }

    private void Update()
    {
        if (mainUI != null)
        {
            getItemData();
        }
        else
        {
            mainUI = UIManager.Instance.GetUI<MainUI>();
        }
    }

    private void getItemData()//���� �ٶ󺸴� ������ ǥ��
    {
        Vector3 sceenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(sceenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, itemCheckDistance, iteractableLayerMask))//��� iteractable layer�� iinteractable�� ������ �ִ�.
        {
            IInteractable iteractable = hit.collider.GetComponent<IInteractable>();
            IsPuzzleCheck(iteractable);

            if (iteractable == null) 
            {
                iteractable = hit.collider.GetComponentInParent<IInteractable>();
                if (iteractable == null) return;
            }
            else if (iteractable == CurrentInteracteable) return;
            CurrentInteracteable = iteractable;
        }
        else CurrentInteracteable = null;

        if (!PuzzleEnter)
        {
            mainUI.ShowPromptUI(CurrentInteracteable);
        }
    }


    private void handleInteractionInput(InputAction.CallbackContext context)//��ȣ�ۿ�� ������ ȸ��
    {
        if (CurrentInteracteable == null) return;
        HandleInputAndPrompt();
        CurrentInteracteable.OnInteract();
        CurrentInteracteable = null;
    }

    public void HandleInputAndPrompt() 
    {
        if (isPuzzle && !PuzzleEnter && !puzzleAccess)
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
    private void IsPuzzleCheck(IInteractable iteractable) 
    {
        if (iteractable as PuzzleBase)
        {
            PuzzleBase puzzle = iteractable as PuzzleBase;
            isPuzzle = true;
            if (puzzle.IsAccess) { puzzleAccess = true; }
            else { puzzleAccess = false; }

        }
        else isPuzzle = false;
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
