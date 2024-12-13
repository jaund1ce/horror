using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class KeypadController : MonoBehaviour, IInteractable
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera keypadCamera; // 키패드 카메라

    [Header("Keypad Settings")]
    public float interactDistance = 3f; // 상호작용 거리

    [Header("Keypad Buttons")]
    public GameObject[] keypadButtons; // 키패드 버튼 오브젝트들 (Key_0~9, Cancel, Enter)
    public GameObject cancelButton; // Cancel 버튼 오브젝트
    public GameObject enterButton; // Enter 버튼 오브젝트
    public string correctCode = "3895"; // 정답 코드

    [Header("Audio Settings")]
    public AudioClip buttonPressSound; // 버튼 누를 때 소리
    public AudioClip successSound; // 정답 소리
    public AudioClip errorSound; // 오답 소리
    public AudioSource audioSource; // 오디오 소스

    public string currentInput = ""; // 현재 입력된 코드
    public bool isUsingKeypad = false; // 키패드 사용 여부
    public bool isPlayerNear = false; // 플레이어가 키패드 근처에 있는지 여부

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player is near the keypad.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the keypad area.");
        }
    }

    public void OnInteract()
    {
        Debug.Log("OnInteract called. isPlayerNear: " + isPlayerNear);

        if (!isPlayerNear) return;

        if (!isUsingKeypad)
        {
            Debug.Log("Calling EnterKeypadView...");
            EnterKeypadView();
        }
        else
        {
            ExitKeypadView();
        }
    }

    private void EnterKeypadView()
    {
        Debug.Log("Entering keypad view...");
        isUsingKeypad = true;

        if (keypadCamera != null)
        {
            keypadCamera.Priority = 11; // 키패드 카메라 활성화
        }

        EnableKeypadButtons(true);

        // 마우스 커서 활성화
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 플레이어 컨트롤 비활성화 (여기서 구현 필요)
    }

    private void ExitKeypadView()
    {
        Debug.Log("Exiting keypad view...");
        isUsingKeypad = false;

        if (keypadCamera != null)
        {
            keypadCamera.Priority = 9; // 키패드 카메라 비활성화
        }

        EnableKeypadButtons(true);

        // 마우스 커서 숨기기 (원래 상태로 복원)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 플레이어 컨트롤 활성화 (여기서 구현 필요)
    }

    private void EnableKeypadButtons(bool enable)
    {
        foreach (GameObject button in keypadButtons)
        {
            Collider buttonCollider = button.GetComponent<Collider>();
            if (buttonCollider != null)
            {
                buttonCollider.enabled = enable;
            }
        }

        if (cancelButton != null)
        {
            Collider cancelCollider = cancelButton.GetComponent<Collider>();
            if (cancelCollider != null)
            {
                cancelCollider.enabled = enable;
            }
        }

        if (enterButton != null)
        {
            Collider enterCollider = enterButton.GetComponent<Collider>();
            if (enterCollider != null)
            {
                enterCollider.enabled = enable;
            }
        }
    }

    public string GetInteractPrompt()
    {
        if (!isUsingKeypad) return "interact";
        else return "exit";
    }

    public void OnButtonPress(string buttonName)
    {
        // 버튼 소리 재생
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
        else if (currentInput.Length < 4) // 숫자 버튼 처리
        {
            currentInput += buttonName;
            Debug.Log("Current Input: " + currentInput);
        }
    }

    public void OnEnterPress()
    {
        if (currentInput == correctCode)
        {
            Debug.Log("Access Granted!");
            // 정답 소리 재생
            if (audioSource != null && successSound != null)
            {
                audioSource.PlayOneShot(successSound);
            }
        }
        else
        {
            Debug.Log("Access Denied!");
            // 오답 소리 재생
            if (audioSource != null && errorSound != null)
            {
                audioSource.PlayOneShot(errorSound);
            }
        }

        currentInput = "";
    }

    public void OnCancelPress()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            Debug.Log("Current Input: " + currentInput);
        }
    }
}




