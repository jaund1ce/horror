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
        playerInputs.Player.Inventory.performed += OnInventoryToggle;
        playerInputs.Player.Menu.performed += OnSystemMenuToggle;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.Inventory.performed -= OnInventoryToggle;
        playerInputs.Player.Menu.performed -= OnSystemMenuToggle;
    }

    private void OnInventoryToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Tab Ű �Է�: Inventory Toggle");
        UIManager.Instance.TogglePopup<Main_PopUP_InventoryUI>();
    }

    private void OnSystemMenuToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("ESC Ű �Է�: System Menu Toggle");
        UIManager.Instance.TogglePopup<Main_PopUP_SystemUI>();
    }
}
