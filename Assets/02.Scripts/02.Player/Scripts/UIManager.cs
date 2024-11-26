using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject escPanel; // ESC Ű�� ����Ǵ� UI
    [SerializeField] private GameObject tabPanel; // TAB Ű�� ����Ǵ� UI

    private PlayerInputs playerInputs;           // Input System�� PlayerInputs ����
    private bool isEscPanelOpen = false;         // ESC �г� ����
    private bool isTabPanelOpen = false;         // TAB �г� ����

    private void Awake()
    {
        // Input System �ʱ�ȭ
        playerInputs = new PlayerInputs();

        // Menu(ESC) �Է� ����
        playerInputs.Player.Menu.performed += _ => HandleMenuInput();

        // Inventory(TAB) �Է� ����
        playerInputs.Player.Inventory.performed += _ => HandleInventoryInput();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    // ESC Ű �Է� ó��
    private void HandleMenuInput()
    {
        if (isTabPanelOpen)
        {
            // TAB �г��� ���� ���� ���, �ݰ� ESC �г� ����
            CloseTabPanel();
            OpenEscPanel();
        }
        else if (isEscPanelOpen)
        {
            // ESC �г��� ���� ���� ���, �ݱ�
            CloseEscPanel();
        }
        else
        {
            // �⺻ ����: ESC �г� ����
            OpenEscPanel();
        }
    }

    // TAB Ű �Է� ó��
    private void HandleInventoryInput()
    {
        if (isEscPanelOpen)
        {
            // ESC �г��� ���� ���� ���, �ݰ� TAB �г� ����
            CloseEscPanel();
            OpenTabPanel();
        }
        else if (isTabPanelOpen)
        {
            // TAB �г��� ���� ���� ���, �ݱ�
            CloseTabPanel();
        }
        else
        {
            // �⺻ ����: TAB �г� ����
            OpenTabPanel();
        }
    }

    // ESC �г� ����
    private void OpenEscPanel()
    {
        escPanel.SetActive(true);
        isEscPanelOpen = true;
        UnlockCursor();
        Time.timeScale = 0f; // ���� �Ͻ� ����
    }

    // ESC �г� �ݱ�
    private void CloseEscPanel()
    {
        escPanel.SetActive(false);
        isEscPanelOpen = false;
        LockCursor();
        Time.timeScale = 1f; // ���� ���� ����
    }

    // TAB �г� ����
    private void OpenTabPanel()
    {
        tabPanel.SetActive(true);
        isTabPanelOpen = true;
        UnlockCursor();
        Time.timeScale = 1f; // ���� �ð� ���� ����
    }

    // TAB �г� �ݱ�
    private void CloseTabPanel()
    {
        tabPanel.SetActive(false);
        isTabPanelOpen = false;
        LockCursor();
        Time.timeScale = 1f; // ���� �ð� ���� ����
    }

    // Ư�� ��ư���� ȣ��: ESC �гΰ� TAB �г� �ݱ�
    public void CloseEscAndTabPanels()
    {
        if (isEscPanelOpen)
        {
            CloseEscPanel();
        }

        if (isTabPanelOpen)
        {
            CloseTabPanel();
        }

        // ���콺 Ŀ���� ��� ���·� ����
        LockCursor();
        Time.timeScale = 1f; // ���� �ð� ���� ����
    }

    // Ŀ�� ��� �� ����
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Ŀ�� ���� �� ǥ��
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
