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

    private PlayerInputs playerInputs;
    private PlayerInputs.PlayerActions playerActions;

    private IInteractable currentInteracteable;
    private bool isPuzzle;
    private bool puzzleEnter;
    private bool puzzleAccess;

    private MainUI mainUI;
    private Player player;

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
    }

    private void Update()
    {
        if (mainUI != null) getItemData();
        else mainUI = UIManager.Instance.GetUI<MainUI>();
    }

    private void getItemData()//현재 바라보는 아이템 표시
    {
        Vector3 sceenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(sceenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, itemCheckDistance, iteractableLayerMask))//모든 iteractable layer은 iinteractable을 가지고 있다.
        {
            IInteractable iteractable = hit.collider.GetComponent<IInteractable>();
            IsPuzzleCheck(iteractable);

            if (iteractable == null) 
            {
                iteractable = hit.collider.GetComponentInParent<IInteractable>();
                if (iteractable == null) return;
            }
            else if (iteractable == currentInteracteable) return;

            currentInteracteable = iteractable;
        }
        else currentInteracteable = null;

        if (!puzzleEnter)
        {
            mainUI.ShowPromptUI(currentInteracteable);
        }
    }


    private void handleInteractionInput(InputAction.CallbackContext context)//상호작용시 아이템 회득
    {
        if (currentInteracteable == null) return;
        HandleInputAndPrompt();
        currentInteracteable.OnInteract();
        currentInteracteable = null;
    }

    public void HandleInputAndPrompt() 
    {
        if (isPuzzle && !puzzleEnter && !puzzleAccess)
        {
            puzzleEnter = true;
            MainGameManager.Instance.Player.Input.InputUnsubscribe();
            mainUI.ShowPromptUI(null);
            return;
        }
        else if (puzzleEnter)
        {
            puzzleEnter = false;
            MainGameManager.Instance.Player.Input.InputSubscribe();
        }
        mainUI.ShowPromptUI(currentInteracteable);
       
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
        if (player == null)
        {
            player = MainGameManager.Instance.Player;

            playerInputs = player.Input.PlayerInputs;
            playerActions = player.Input.PlayerActions;
        }      

        playerActions.Interaction.started += handleInteractionInput;
    }

    private void OnDisable()
    {
        playerActions.Interaction.started -= handleInteractionInput;
    }
}
