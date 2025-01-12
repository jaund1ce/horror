using UnityEngine;

public class ManualUI : PopupUI
{
    public GameObject targetObject;


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

    private void Update()
    {
        UnlockCursor();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void OpenBigManualUI()
    {
        if (targetObject != null) // targetObject가 null이 아니라면(Destroy, 아예 오브젝트가 없지 않다면)
        {
            bool isActive = targetObject.activeSelf; // activeSelf = targetObject의 SetAtive상태의(True, False) bool 지역 변수 
            targetObject.SetActive(!isActive); // activeSelf(targetObject의 SetAtive상태(True, False) )의 반대로 SetActive 한다
        }
    }


}