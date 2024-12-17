using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using static System.Net.WebRequestMethods;

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
    public AudioClip AccessSound; // ���� ����
    public AudioClip DeniedSound; // ���� ����
    public AudioSource audioSource; // ����� �ҽ�

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


    private string currentInput = ""; // ���� �Էµ� �ڵ�
    public bool isUsingKeypad = false; // Ű�е� Ȱ��ȭ ����

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
                audioSource.PlayOneShot(AccessSound); // ���� �Ҹ� ���
                isAccess = true;
            }
        }
        else
        {
            if (audioSource != null && DeniedSound != null)
            {
                currentCoroutine = StartCoroutine(Denied());
                audioSource.PlayOneShot(DeniedSound); // ���� �Ҹ� ���
            }
        }

        currentInput = ""; // �Է� �ʱ�ȭ
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
