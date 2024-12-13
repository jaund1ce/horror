using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class KeypadController : MonoBehaviour
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

    private string currentInput = ""; // 현재 입력된 코드
    private bool isUsingKeypad = false; // 키패드 사용 여부
    public bool playerNearby = false; // 플레이어가 키패드 근처에 있는지 여부

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player entered keypad interaction area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Player exited keypad interaction area.");
        }
    }

    void Update()
    {
        if (playerNearby && !isUsingKeypad && Input.GetKeyDown(KeyCode.E))
        {
            EnterKeypadView();
        }
        else if (isUsingKeypad && Input.GetKeyDown(KeyCode.Q))
        {
            ExitKeypadView();
        }

        // 마우스 클릭 처리
        if (isUsingKeypad && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Main Camera에서 Ray 발사
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandleButtonClick(hit.collider.gameObject);
            }
        }
    }

    private void HandleButtonClick(GameObject clickedObject)
    {
        foreach (GameObject button in keypadButtons)
        {
            if (clickedObject == button)
            {
                OnButtonPress(button.name.Replace("Key_", ""));
                return;
            }
        }

        if (clickedObject == cancelButton)
        {
            OnButtonPress("Cancel");
        }
        else if (clickedObject == enterButton)
        {
            OnButtonPress("Enter");
        }
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
}




