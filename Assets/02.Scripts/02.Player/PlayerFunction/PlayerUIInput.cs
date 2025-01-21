using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerUIInput : MonoBehaviour
{
    public static bool FisrtStart = false;
    private Player player;
    private PlayerInputs playerInputs;
    private PlayerConditionController playerConditionController;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInputs = player.Input.PlayerInputs;
        playerConditionController = GetComponent<PlayerConditionController>();
        if (playerConditionController == null)
        {
            Debug.LogWarning("Health 컴포넌트를 찾을 수 없습니다. OnDie 이벤트는 등록되지 않습니다.");
        }
    }

    private void Start()
    {
        if (FisrtStart)
        {
            return;
        }
        UIManager.Instance.Show<ManualUI>();
        FisrtStart = true;
    }

    private void OnEnable()
    {
        playerInputs.Player.Inventory.performed += OnInventory;
        playerInputs.Player.Menu.performed += OnSystemMenu;
        if (playerConditionController != null)
        {
            playerConditionController.OnDie += OnDieUI;
        }
    }

    private void OnDisable()
    {
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
        if (playerConditionController != null)
        {
            playerConditionController.OnDie -= OnDieUI;
        }
    }

    private void OnInventory(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<SystemUI>();
        UIManager.Instance.Show<InventoryUI>();
    }

    private void OnSystemMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<InventoryUI>();
        UIManager.Instance.Show<SystemUI>();
    }

    private void OnDieUI()
    {
        playerInputs.Disable();
        UIManager.Instance.Hide<PaperUI>();
        UIManager.Instance.Hide<InventoryUI>();
        SoundManger.Instance.ResetAllSounds();
        StartCoroutine(Delay(2.0f));
    }

    IEnumerator Delay(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        UIManager.Instance.Show<DieUI>();
    }
}
