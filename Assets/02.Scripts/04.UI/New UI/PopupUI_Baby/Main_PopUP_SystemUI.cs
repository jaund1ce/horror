using UnityEngine;

public class Main_PopUP_SystemUI : PopupUI
{
    // �ʿ��ϴٸ� �߰����� ���� ����
    public override void OpenUI()
    {
        base.OpenUI();
        Debug.Log("Main_PopUP_SystemUI ����");
    }

    public override void CloseUI()
    {
        base.CloseUI();
        Debug.Log("Main_PopUP_SystemUI ����");
    }
}
