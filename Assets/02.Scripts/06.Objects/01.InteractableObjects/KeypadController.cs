using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class KeypadController : MonoBehaviour, IInteractable
{
    [Header("Camera Settings")]
    public CinemachineVirtualCamera keypadCamera; // Ű�е� ī�޶�

    [Header("Keypad Settings")]
    public float interactDistance = 3f; // ��ȣ�ۿ� �Ÿ�

    [Header("Keypad Buttons")]
    public GameObject[] keypadButtons; // Ű�е� ��ư ������Ʈ�� (Key_0~9, Cancel, Enter)
    public GameObject cancelButton; // Cancel ��ư ������Ʈ
    public GameObject enterButton; // Enter ��ư ������Ʈ
    public string correctCode = "3895"; // ���� �ڵ�

    [Header("Audio Settings")]
    public AudioClip buttonPressSound; // ��ư ���� �� �Ҹ�
    public AudioClip successSound; // ���� �Ҹ�
    public AudioClip errorSound; // ���� �Ҹ�
    public AudioSource audioSource; // ����� �ҽ�

    public string currentInput = ""; // ���� �Էµ� �ڵ�
    public bool isUsingKeypad = false; // Ű�е� ��� ����
    public bool isPlayerNear = false; // �÷��̾ Ű�е� ��ó�� �ִ��� ����

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
            keypadCamera.Priority = 11; // Ű�е� ī�޶� Ȱ��ȭ
        }

        EnableKeypadButtons(true);

        // ���콺 Ŀ�� Ȱ��ȭ
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // �÷��̾� ��Ʈ�� ��Ȱ��ȭ (���⼭ ���� �ʿ�)
    }

    private void ExitKeypadView()
    {
        Debug.Log("Exiting keypad view...");
        isUsingKeypad = false;

        if (keypadCamera != null)
        {
            keypadCamera.Priority = 9; // Ű�е� ī�޶� ��Ȱ��ȭ
        }

        EnableKeypadButtons(true);

        // ���콺 Ŀ�� ����� (���� ���·� ����)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // �÷��̾� ��Ʈ�� Ȱ��ȭ (���⼭ ���� �ʿ�)
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
        // ��ư �Ҹ� ���
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
        else if (currentInput.Length < 4) // ���� ��ư ó��
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
            // ���� �Ҹ� ���
            if (audioSource != null && successSound != null)
            {
                audioSource.PlayOneShot(successSound);
            }
        }
        else
        {
            Debug.Log("Access Denied!");
            // ���� �Ҹ� ���
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




