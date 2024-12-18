using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using static System.Net.WebRequestMethods;

public class PuzzleKeypad : PuzzleBase
{
    [Header("Keypad Settings")]
    public GameObject[] keypadButtons; // 키패드 버튼 오브젝트들
    public GameObject cancelButton; // Cancel 버튼
    public GameObject enterButton; // Enter 버튼
    public string correctCode = "3895"; // 정답 코드
    public AudioClip buttonPressSound; // 버튼 클릭 소리
    public AudioClip AccessSound; // 성공 사운드
    public AudioClip DeniedSound; // 실패 사운드
    public AudioSource audioSource; // 오디오 소스
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


    private string currentInput = ""; // 현재 입력된 코드

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        keypadRenderer = GetComponent<MeshRenderer>();
        keypadRenderer.material.DisableKeyword(txtBgLightKeyword);
    }


    public override void OnInteract()
    {
        if (isAccess) return;
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
            audioSource.PlayOneShot(buttonPressSound); // 버튼 소리 재생
        }

        if (buttonName == "Enter")
        {
            OnEnterPress();
        }
        else if (buttonName == "Cancel")
        {
            OnCancelPress();
        }
        else if (currentInput.Length < 4) // 숫자 버튼
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

        currentInput = ""; // 입력 초기화
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


        text.color = baseColor;
        currentCoroutine = null;
        //락도어에 불리언값 변경 코드 작성
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
