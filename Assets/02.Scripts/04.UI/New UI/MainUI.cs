using UnityEngine;

public class MainUI : BaseUI
{
    public PromptUI promptUI;

    public override void OpenUI()
    {
        base.OpenUI();
        promptUI.CloseUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void ShowPromptUI(IInteractable CurrentInteracteable)
    {
        if (CurrentInteracteable == null)
        {
            promptUI.CloseUI();
        }
        else 
        {
            promptUI.OpenUI();
            promptUI.SetPromptText(CurrentInteracteable.GetInteractPrompt());
        }
        
    }
}
