using UnityEngine;

public class PlayerUIInput : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private PlayerConditionController playerConditionController;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerConditionController = GetComponent<PlayerConditionController>();
        if (playerConditionController == null)
        {
            Debug.LogWarning("Health ������Ʈ�� ã�� �� �����ϴ�. OnDie �̺�Ʈ�� ��ϵ��� �ʽ��ϴ�.");
        }
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Inventory.performed += OnInventory;
        playerInputs.Player.Menu.performed += OnSystemMenu;
        if (playerConditionController != null)
        {
            playerConditionController.OnDie += OnDieUI; // �̺�Ʈ ����
        }
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
        if (playerConditionController != null)
        {
            playerConditionController.OnDie -= OnDieUI; // �̺�Ʈ ���� ����
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
