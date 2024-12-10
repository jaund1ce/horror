using UnityEngine;

public class SystemUI : PopupUI
{
    public override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Time.timeScale = 1f;
    }
    // �ʿ��ϴٸ� �߰����� ���� ����
    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }


}