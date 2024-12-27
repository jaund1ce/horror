using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PuzzleKeypad : PuzzleBase
{
    [Header("Keypad Settings")]
    public GameObject[] keypadButtons; 
    public GameObject cancelButton; 
    public GameObject enterButton; 
    public string correctCode = "3895"; // 정답 코드
    public AudioClip buttonPressSound; 
    public AudioClip AccessSound;
    public AudioClip DeniedSound; 
    public AudioSource audioSource; 
    public LockedDoor LockDoor;

    private TextMeshPro text;
    private MeshRenderer keypadRenderer;
    private Color baseColor = Color.black;
    private Color accessColor = Color.green;
    private string accessTxt = "ACCESS";
    private Color deniedColor = Color.red;
    private string deniedTxt = "DENIED";
    private string txtBgLightKeyword = "_EMISSION";
    private Coroutine currentCoroutine;


    private string currentInput = ""; 

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        keypadRenderer = GetComponent<MeshRenderer>();
        keypadRenderer.material.DisableKeyword(txtBgLightKeyword);
    }


    public override void OnInteract()
    {
        if (!isUsingPuzzle)
        {
            keypadRenderer.material.EnableKeyword(txtBgLightKeyword);
            EnterPuzzleView();
        }
        else
        {
            keypadRenderer.material.DisableKeyword(txtBgLightKeyword);
            ExitPuzzleView();
        }
    }

    protected override void EnterPuzzleView()
    {
        base.EnterPuzzleView();
        isUsingPuzzle = true;
    }

    protected override void ExitPuzzleView()
    {
        base.ExitPuzzleView();
        isUsingPuzzle = false;
    }

    public override string GetInteractPrompt()
    {
        return promptTxt;
    }

    public void OnButtonPress(string buttonName)
    {
        if (isAccess) return;

        if (currentCoroutine != null) 
        {
            StopCoroutine(currentCoroutine);
            text.color = baseColor;
        }
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound); 
        }

        if (buttonName == "Enter")
        {
            OnEnterPress();
        }
        else if (buttonName == "Cancel")
        {
            OnCancelPress();
        }
        else if (currentInput.Length < 4)
        {
            currentInput += buttonName;
            text.text = currentInput;
        }
    }

    private void OnEnterPress()
    {
        if (currentInput == correctCode)
        {
            if (audioSource != null && AccessSound != null)
            {
                currentCoroutine = StartCoroutine(Access());
                audioSource.PlayOneShot(AccessSound); // 정답 소리 재생
                isAccess = true;
            }
        }
        else
        {
            if (audioSource != null && DeniedSound != null)
            {
                currentCoroutine = StartCoroutine(Denied());
                audioSource.PlayOneShot(DeniedSound); // 실패 소리 재생
            }
        }

        currentInput = ""; 
        text.text = currentInput;
    }

    private void OnCancelPress()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            text.text = currentInput;
        }
    }

    private IEnumerator Access()
    {
        float coroutineTime = 0;
        while (coroutineTime < 1f)
        {
            text.color = accessColor;
            if (text.text == accessTxt)
            {
                text.text = null;
            }
            else
            {
                text.text = accessTxt;
            }


            yield return new WaitForSeconds(0.2f);
            coroutineTime += 0.2f;
        }


        text.text = accessTxt;
        currentCoroutine = null;
        if (LockDoor.IsLocked) LockDoor.IsLocked = false;
        MainGameManager.Instance.Player.Interact.HandleInputAndPrompt();
        ExitPuzzleView();
    }

    private IEnumerator Denied() 
    {
        float coroutineTime = 0;
        while (coroutineTime < 1f) 
        {
            text.color = deniedColor;
            if (text.text == deniedTxt)
            {
                text.text = null;
            }
            else 
            {
                text.text = deniedTxt;
            }
            

            yield return new WaitForSeconds(0.2f);
            coroutineTime += 0.2f;
        }


        text.color = baseColor;
        currentCoroutine = null;
    }
}
