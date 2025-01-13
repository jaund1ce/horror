using UnityEngine;

public class InventoryUI : PopupUI
{
    public MainGameManager gameManager;
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    // 필요하다면 추가적인 동작 구현
    public override void OpenUI()
    { 
        base.OpenUI();
    }

    public override void CloseUI()
    {
       base .CloseUI();
    }

    public void OnDocumentUI()
    {
        if (gameManager.component != null)
        {
            if (gameManager.getNewPaper == true)
            {
                gameManager.component.enabled = false;
                gameManager.getNewPaper = false;
            }
        }
        UIManager.Instance.Hide<InventoryUI>();
        UIManager.Instance.Show<PaperUI>();
    }
}
