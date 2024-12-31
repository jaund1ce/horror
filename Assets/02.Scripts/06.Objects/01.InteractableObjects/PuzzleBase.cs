using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class PuzzleBase : MonoBehaviour , IInteractable
{
    public CinemachineVirtualCamera PuzzleCamera;
    [HideInInspector]public bool IsAccess;
    protected string promptTxt = "Interact";
    protected bool isUsingPuzzle = false;

    public virtual string GetInteractPrompt()
    {
        return promptTxt;
    }

    public virtual void OnInteract()
    {
        //if (isAccess) return;
        if (!isUsingPuzzle)
        {
            EnterPuzzleView();
        }
        else
        {
            ExitPuzzleView();
        }
    }

    protected virtual void EnterPuzzleView() 
    {
        if (PuzzleCamera != null)
        {
            PuzzleCamera.Priority = 11; // 카메라 활성화
        }

        Cursor.visible = true; // 마우스 커서 활성화
        Cursor.lockState = CursorLockMode.None;
    }

    protected virtual void ExitPuzzleView()
    {
        if (PuzzleCamera != null)
        {
            PuzzleCamera.Priority = 9; // 카메라 비활성화
        }

        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked;
    }
}

