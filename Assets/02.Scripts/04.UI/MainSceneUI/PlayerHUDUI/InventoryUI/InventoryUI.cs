using UnityEngine;

public class InventoryUI : PopupUI
{
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    // �ʿ��ϴٸ� �߰����� ���� ����
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

        UIManager.Instance.Hide<InventoryUI>();
        UIManager.Instance.Show<PaperUI>();
    }
}
