using UnityEngine;

public class Main_Player_UIInput : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private PlayerConditionController health;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        health = GetComponent<PlayerConditionController>();
        if (health == null)
        {
            Debug.LogWarning("Health 컴포넌트를 찾을 수 없습니다. OnDie 이벤트는 등록되지 않습니다.");
        }
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Inventory.performed += OnInventory;
        playerInputs.Player.Menu.performed += OnSystemMenu;
        if (health != null)
        {
            health.OnDie += OnDieUI; // 이벤트 구독
        }
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
        if (health != null)
        {
            health.OnDie -= OnDieUI; // 이벤트 구독 해제
        }
    }


        private void OnInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Tab 키 입력");
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<SystemUI>();
        UIManager.Instance.Show<InventoryUI>();
    }

    private void OnSystemMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("ESC 키 입력");
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<InventoryUI>();
        UIManager.Instance.Show<SystemUI>();

    }

    private void OnDieUI()
    {
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<InventoryUI>();
        UIManager.Instance.Show<DieUI>();
    }
}
