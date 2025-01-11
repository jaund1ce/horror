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
    // �ʿ��ϴٸ� �߰����� ���� ����
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
        if (targetObject != null) // targetObject�� null�� �ƴ϶��(Destroy, �ƿ� ������Ʈ�� ���� �ʴٸ�)
        {
            bool isActive = targetObject.activeSelf; // activeSelf = targetObject�� SetAtive������(True, False) bool ���� ���� 
            targetObject.SetActive(!isActive); // activeSelf(targetObject�� SetAtive����(True, False) )�� �ݴ�� SetActive �Ѵ�
        }
    }


}