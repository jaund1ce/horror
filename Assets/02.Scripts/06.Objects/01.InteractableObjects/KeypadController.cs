using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    public AudioClip successSound; // 성공 사운드
    public AudioClip errorSound; // 실패 사운드
    public AudioSource audioSource; // 오디오 소스

    private string currentInput = ""; // 현재 입력된 코드
    public bool isUsingKeypad = false; // 키패드 활성화 여부

    


    public void OnInteract()
    {
        if (!isUsingKeypad)
        {
            EnterKeypadView();
        }
        else
        {
            ExitKeypadView();
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
        //if (!isUsingKeypad) return isPlayerNear ? "interact" : "";
        return "interact";
    }

    public void OnButtonPress(string buttonName)
    {
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
            Debug.Log("Current Input: " + currentInput);
        }
    }

    private void OnEnterPress()
    {
        if (currentInput == correctCode)
        {
            Debug.Log("Access Granted!");
            if (audioSource != null && successSound != null)
            {
                audioSource.PlayOneShot(successSound); // 정답 소리 재생
            }
        }
        else
        {
            Debug.Log("Access Denied!");
            if (audioSource != null && errorSound != null)
            {
                audioSource.PlayOneShot(errorSound); // 실패 소리 재생
            }
        }

        currentInput = ""; // 입력 초기화
    }

    private void OnCancelPress()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            Debug.Log("Current Input: " + currentInput);
        }
    }
}
