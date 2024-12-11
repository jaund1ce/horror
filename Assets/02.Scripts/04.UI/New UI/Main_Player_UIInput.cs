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
            Debug.LogWarning("Health ������Ʈ�� ã�� �� �����ϴ�. OnDie �̺�Ʈ�� ��ϵ��� �ʽ��ϴ�.");
        }
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Inventory.performed += OnInventory;
        playerInputs.Player.Menu.performed += OnSystemMenu;
        if (health != null)
        {
            health.OnDie += OnDieUI; // �̺�Ʈ ����
        }
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
        if (health != null)
        {
            health.OnDie -= OnDieUI; // �̺�Ʈ ���� ����
        }
    }


        private void OnInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Tab Ű �Է�");
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<SystemUI>();
        UIManager.Instance.Show<InventoryUI>();
    }

    private void OnSystemMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("ESC Ű �Է�");
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
