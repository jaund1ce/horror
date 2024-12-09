using UnityEngine;

public class SystemUI : PopupUI
{
    public override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Time.timeScale = 1f;
    }
    // 필요하다면 추가적인 동작 구현
    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }
}