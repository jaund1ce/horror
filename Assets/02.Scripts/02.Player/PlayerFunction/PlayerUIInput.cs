using System;
using System.Collections;
using System.Threading;
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
            Debug.LogWarning("Health 컴포넌트를 찾을 수 없습니다. OnDie 이벤트는 등록되지 않습니다.");
        }
    }

    private void Start()
    {

        UIManager.Instance.Show<MainUI>();
        if (MainGameManager.Instance.fisrtStart == 0)
        {
            UIManager.Instance.Show<ManualUI>();
        }
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.Inventory.performed += OnInventory;
        playerInputs.Player.Menu.performed += OnSystemMenu;
        if (playerConditionController != null)
        {
            playerConditionController.OnDie += OnDieUI; // 이벤트 구독
        }
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
        if (playerConditionController != null)
        {
            playerConditionController.OnDie -= OnDieUI; // 이벤트 구독 해제
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
        playerInputs.Player.Inventory.performed -= OnInventory;
        playerInputs.Player.Menu.performed -= OnSystemMenu;
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
