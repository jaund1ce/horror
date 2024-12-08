using UnityEngine;

public class Main_PopUP_SystemUI : PopupUI
{
    // 필요하다면 추가적인 동작 구현
    public override void OpenUI()
    {
        base.OpenUI();
        Debug.Log("Main_PopUP_SystemUI 열림");
    }

    public override void CloseUI()
    {
        base.CloseUI();
        Debug.Log("Main_PopUP_SystemUI 닫힘");
    }
}
