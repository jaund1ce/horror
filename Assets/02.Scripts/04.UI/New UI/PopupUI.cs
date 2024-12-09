using UnityEngine;

public class PopupUI : BaseUI
{
    public virtual void OnEnable()
    {
        UnlockCursor();
    }
    public virtual void OnDestroy()
    {
        LockCursor();
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
