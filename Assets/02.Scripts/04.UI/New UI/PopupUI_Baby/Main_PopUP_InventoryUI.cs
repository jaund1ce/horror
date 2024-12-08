using UnityEngine;

public class Main_PopUP_InventoryUI : PopupUI
{
    // 필요하다면 추가적인 동작 구현
    public override void OpenUI()
    {
        base.OpenUI();
        Debug.Log("Main_PopUP_InventoryUI 열림");
    }

    public override void CloseUI()
    {
        base.CloseUI();
        Debug.Log("Main_PopUP_InventoryUI 닫힘");
    }
}
