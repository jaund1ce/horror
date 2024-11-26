using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject escPanel; // ESC 키로 제어되는 UI
    [SerializeField] private GameObject tabPanel; // TAB 키로 제어되는 UI

    private PlayerInputs playerInputs;           // Input System의 PlayerInputs 참조
    private bool isEscPanelOpen = false;         // ESC 패널 상태
    private bool isTabPanelOpen = false;         // TAB 패널 상태

    private void Awake()
    {
        // Input System 초기화
        playerInputs = new PlayerInputs();

        // Menu(ESC) 입력 연결
        playerInputs.Player.Menu.performed += _ => HandleMenuInput();

        // Inventory(TAB) 입력 연결
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

    // ESC 키 입력 처리
    private void HandleMenuInput()
    {
        if (isTabPanelOpen)
        {
            // TAB 패널이 열려 있을 경우, 닫고 ESC 패널 열기
            CloseTabPanel();
            OpenEscPanel();
        }
        else if (isEscPanelOpen)
        {
            // ESC 패널이 열려 있을 경우, 닫기
            CloseEscPanel();
        }
        else
        {
            // 기본 동작: ESC 패널 열기
            OpenEscPanel();
        }
    }

    // TAB 키 입력 처리
    private void HandleInventoryInput()
    {
        if (isEscPanelOpen)
        {
            // ESC 패널이 열려 있을 경우, 닫고 TAB 패널 열기
            CloseEscPanel();
            OpenTabPanel();
        }
        else if (isTabPanelOpen)
        {
            // TAB 패널이 열려 있을 경우, 닫기
            CloseTabPanel();
        }
        else
        {
            // 기본 동작: TAB 패널 열기
            OpenTabPanel();
        }
    }

    // ESC 패널 열기
    private void OpenEscPanel()
    {
        escPanel.SetActive(true);
        isEscPanelOpen = true;
        UnlockCursor();
        Time.timeScale = 0f; // 게임 일시 정지
    }

    // ESC 패널 닫기
    private void CloseEscPanel()
    {
        escPanel.SetActive(false);
        isEscPanelOpen = false;
        LockCursor();
        Time.timeScale = 1f; // 게임 정상 상태
    }

    // TAB 패널 열기
    private void OpenTabPanel()
    {
        tabPanel.SetActive(true);
        isTabPanelOpen = true;
        UnlockCursor();
        Time.timeScale = 1f; // 게임 시간 정상 유지
    }

    // TAB 패널 닫기
    private void CloseTabPanel()
    {
        tabPanel.SetActive(false);
        isTabPanelOpen = false;
        LockCursor();
        Time.timeScale = 1f; // 게임 시간 정상 유지
    }

    // 특정 버튼에서 호출: ESC 패널과 TAB 패널 닫기
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

        // 마우스 커서를 잠금 상태로 설정
        LockCursor();
        Time.timeScale = 1f; // 게임 시간 정상 유지
    }

    // 커서 잠금 및 숨김
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 커서 해제 및 표시
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
