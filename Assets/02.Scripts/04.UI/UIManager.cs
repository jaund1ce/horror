using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : mainSingleton<UIManager>
{
    [SerializeField] private GameObject escPanel;
    [SerializeField] private GameObject tabPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject docPanel;

    private PlayerInputs playerInputs;
    private bool isEscPanelOpen = false;
    private bool isTabPanelOpen = false;
    private bool isSettingPanelOpen = false;
    private bool isinteractPanelOpen = false;
    private bool isDocPanelOpen = false;

    protected override void Awake()
    {
        base.Awake();
        playerInputs = new PlayerInputs();
        playerInputs.Enable();

        playerInputs.Player.Menu.performed += _ => HandleMenuInput();
        playerInputs.Player.Inventory.performed += _ => HandleInventoryInput();
    }

    protected override void OnEnable()
    {
        playerInputs.Enable();
    }

    protected override void OnDisable()
    {
        playerInputs.Disable();
    }

    public void HandleMenuInput()
    {
        if (isSettingPanelOpen)
        {
            CloseSettingPanel();
            OpenEscPanel();
        }
        else if (isTabPanelOpen)
        {
            CloseTabPanel();
            OpenEscPanel();
        }
        else if (isEscPanelOpen)
        {
            CloseEscPanel();
        }
        else
        {
            OpenEscPanel();
        }
    }

    public void HandleInventoryInput()
    {
        if (isSettingPanelOpen)
        {
            CloseSettingPanel();
            OpenTabPanel();
        }
        else if (isEscPanelOpen)
        {
            CloseEscPanel();
            OpenTabPanel();
        }
        else if (isTabPanelOpen)
        {
            CloseTabPanel();
        }
        else
        {
            OpenTabPanel();
        }
    }

    public void HandleSettingInput()
    {
        if (isEscPanelOpen)
        {
            CloseEscPanel();
            OpenSettingPanel();
        }
        else if (isTabPanelOpen)
        {
            CloseTabPanel();
            OpenSettingPanel();
        }
        else if (isSettingPanelOpen)
        {
            CloseSettingPanel();
        }
        else
        {
            OpenSettingPanel();
        }
    }

    private void OpenEscPanel()
    {
        if (escPanel != null)
        {
            escPanel.SetActive(true);
            isEscPanelOpen = true;
            UnlockCursor();
            Time.timeScale = 0f;
        }
    }

    private void CloseEscPanel()
    {
        if (escPanel != null)
        {
            escPanel.SetActive(false);
            isEscPanelOpen = false;
            LockCursor();
            Time.timeScale = 1f;
        }
    }

    private void OpenTabPanel()
    {
        if (tabPanel != null)
        {
            tabPanel.SetActive(true);
            isTabPanelOpen = true;
            UnlockCursor();
            Time.timeScale = 1f;
        }
    }

    private void CloseTabPanel()
    {
        if (tabPanel != null)
        {
            tabPanel.SetActive(false);
            isTabPanelOpen = false;
            LockCursor();
            Time.timeScale = 1f;
        }
    }

    private void OpenSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
            isSettingPanelOpen = true;
            UnlockCursor();
            Time.timeScale = 0f;
        }
    }

    private void CloseSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
            isSettingPanelOpen = false;
            LockCursor();
            Time.timeScale = 1f;
        }
    }

    public void OpenDocPanel()
    {
        if (docPanel != null)
        {
            docPanel.SetActive(true);
            isDocPanelOpen = true;
            UnlockCursor();
            Time.timeScale = 1f;
        }
    }

    public void CloseDocPanel()
    {
        if (docPanel != null)
        {
            docPanel.SetActive(false);
            isDocPanelOpen = false;
            LockCursor();
            Time.timeScale = 1f;
        }
    }

    public void ClosePanels()
    {
        if (isEscPanelOpen)
        {
            CloseEscPanel();
        }

        if (isTabPanelOpen)
        {
            CloseTabPanel();
        }

        if (isSettingPanelOpen)
        {
            CloseSettingPanel();
        }

        LockCursor();
        Time.timeScale = 1f;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpeninteractPanel()
    {
        if (interactPanel != null)
        {
            interactPanel.SetActive(true);
            isinteractPanelOpen = true;
            UnlockCursor();
        }
    }

    public void CloseInteractPanel()
    {
        if (interactPanel != null)
        {
            interactPanel.SetActive(false);
            isinteractPanelOpen = false;
            UnlockCursor();
        }
    }
}
