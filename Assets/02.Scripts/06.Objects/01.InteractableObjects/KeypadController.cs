using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using static System.Net.WebRequestMethods;

public class KeypadController : MonoBehaviour, IInteractable
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera keypadCamera; // 키패드 카메라

    [Header("Keypad Settings")]
    public GameObject[] keypadButtons; // 키패드 버튼 오브젝트들
    public GameObject cancelButton; // Cancel 버튼
    public GameObject enterButton; // Enter 버튼
    public string correctCode = "3895"; // 정답 코드
    public AudioClip buttonPressSound; // 버튼 클릭 소리
    public AudioClip AccessSound; // 성공 사운드
    public AudioClip DeniedSound; // 실패 사운드
    public AudioSource audioSource; // 오디오 소스

    private TextMeshPro text;
    private MeshRenderer keypadRenderer;
    private Color baseColor = Color.black;
    private Color accessColor = Color.green;
    private string accessTxt = "ACCESS";
    private Color deniedColor = Color.red;
    private string deniedTxt = "DENIED";
    private string txtBgLightKeyword = "_EMISSION";
    private Coroutine currentCoroutine;
    private bool isAccess;


    private string currentInput = ""; // 현재 입력된 코드
    public bool isUsingKeypad = false; // 키패드 활성화 여부

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        keypadRenderer = GetComponent<MeshRenderer>();
        keypadRenderer.material.DisableKeyword(txtBgLightKeyword);
    }


    public void OnInteract()
    {
        if (!isUsingKeypad)
        {
            keypadRenderer.material.EnableKeyword(txtBgLightKeyword);
            EnterKeypadView();
            MainGameManager.Instance.Player.Input.playerActions.EquipmentUse.started -= MainGameManager.Instance.Player.Input.EquipMent.OnAttackInput;
        }
        else
        {
            keypadRenderer.material.DisableKeyword(txtBgLightKeyword);
            ExitKeypadView();
            MainGameManager.Instance.Player.Input.playerActions.EquipmentUse.started += MainGameManager.Instance.Player.Input.EquipMent.OnAttackInput;
        }
    }

    private void EnterKeypadView()
    {
        isUsingKeypad = true;

        if (keypadCamera != null)
        {
            keypadCamera.Priority = 11; // 카메라 활성화
        }

        Cursor.visible = true; // 마우스 커서 활성화
        Cursor.lockState = CursorLockMode.None;
    }

    private void ExitKeypadView()
    {
        isUsingKeypad = false;

        if (keypadCamera != null)
        {
            keypadCamera.Priority = 9; // 카메라 비활성화
        }

        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked;
    }

    public string GetInteractPrompt()
    {
        return !isUsingKeypad ? "interact" : null;
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
            Debug.Log("Current Input: " + currentInput);
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
            Debug.Log("Current Input: " + currentInput);
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
        ExitKeypadView();
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
