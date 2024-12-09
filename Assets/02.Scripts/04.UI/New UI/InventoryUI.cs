using UnityEngine;

public class InventoryUI : PopupUI
{
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
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
}
