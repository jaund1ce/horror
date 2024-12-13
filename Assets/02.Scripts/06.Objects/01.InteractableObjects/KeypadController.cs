using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class KeypadController : MonoBehaviour
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

    private string currentInput = ""; // ���� �Էµ� �ڵ�
    private bool isUsingKeypad = false; // Ű�е� ��� ����
    public bool playerNearby = false; // �÷��̾ Ű�е� ��ó�� �ִ��� ����

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

        // ���콺 Ŭ�� ó��
        if (isUsingKeypad && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Main Camera���� Ray �߻�
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
}




