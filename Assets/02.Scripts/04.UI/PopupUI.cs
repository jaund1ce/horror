using UnityEngine;

public class PopupUI : BaseUI
{
    private PlayerController playerController;
    public virtual void OnEnable()
    {
        playerController = FindObjectOfType<PlayerController>(); 
        if (playerController != null)
        {
            playerController.LockRotate(); // 회전 잠금
        }
        UnlockCursor();
        MainGameManager.Instance.Player.Input.InputUnsubscribe();
    }
    public virtual void OnDisable()
    {
        if (playerController != null)
        {
            playerController.UnLockRotate(); // 회전 잠금 해제
        }
        LockCursor();
        MainGameManager.Instance.Player.Input.InputSubscribe();
    }
    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
