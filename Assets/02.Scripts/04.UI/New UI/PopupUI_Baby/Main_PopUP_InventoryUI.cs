using UnityEngine;

public class Main_PopUP_InventoryUI : PopupUI
{
    // �ʿ��ϴٸ� �߰����� ���� ����
    public override void OpenUI()
    {
        base.OpenUI();
        Debug.Log("Main_PopUP_InventoryUI ����");
    }

    public override void CloseUI()
    {
        base.CloseUI();
        Debug.Log("Main_PopUP_InventoryUI ����");
    }
}
