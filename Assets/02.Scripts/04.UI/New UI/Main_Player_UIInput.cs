using UnityEngine;

public class Main_Player_UIInput : MonoBehaviour
{
    private PlayerInputs playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Inventory.performed += OnInventory;
        playerInputs.Player.Menu.performed += OnSystemMenu;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
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
}
