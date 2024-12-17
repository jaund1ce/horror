using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KeypadController : MonoBehaviour, IInteractable
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera keypadCamera; // Ű�е� ī�޶�

    [Header("Keypad Settings")]
    public GameObject[] keypadButtons; // Ű�е� ��ư ������Ʈ��
    public GameObject cancelButton; // Cancel ��ư
    public GameObject enterButton; // Enter ��ư
    public string correctCode = "3895"; // ���� �ڵ�
    public AudioClip buttonPressSound; // ��ư Ŭ�� �Ҹ�
    public AudioClip successSound; // ���� ����
    public AudioClip errorSound; // ���� ����
    public AudioSource audioSource; // ����� �ҽ�

    private string currentInput = ""; // ���� �Էµ� �ڵ�
    public bool isUsingKeypad = false; // Ű�е� Ȱ��ȭ ����

    


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
            keypadCamera.Priority = 11; // ī�޶� Ȱ��ȭ
        }

        Cursor.visible = true; // ���콺 Ŀ�� Ȱ��ȭ
        Cursor.lockState = CursorLockMode.None;
    }

    private void ExitKeypadView()
    {
        isUsingKeypad = false;

        if (keypadCamera != null)
        {
            keypadCamera.Priority = 9; // ī�޶� ��Ȱ��ȭ
        }

        Cursor.visible = false; // ���콺 Ŀ�� ����
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
            audioSource.PlayOneShot(buttonPressSound); // ��ư �Ҹ� ���
        }

        if (buttonName == "Enter")
        {
            OnEnterPress();
        }
        else if (buttonName == "Cancel")
        {
            OnCancelPress();
        }
        else if (currentInput.Length < 4) // ���� ��ư
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
                audioSource.PlayOneShot(successSound); // ���� �Ҹ� ���
            }
        }
        else
        {
            Debug.Log("Access Denied!");
            if (audioSource != null && errorSound != null)
            {
                audioSource.PlayOneShot(errorSound); // ���� �Ҹ� ���
            }
        }

        currentInput = ""; // �Է� �ʱ�ȭ
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
