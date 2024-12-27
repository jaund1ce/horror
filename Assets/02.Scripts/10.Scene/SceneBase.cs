using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    public Transform uiTransform; // UI ������Ʈ�� ��ġ�� �θ� Transform�� �����մϴ�.
                                  // UIManager�� �̸� �����Ͽ� �������� ������ UI�� ������ ��ġ�� ��ġ�մϴ�
                                  // �� ��ȯ �ÿ��� ������ ������ �����ϱ� ���� ���˴ϴ�.
    public Transform mapTransform;

    protected virtual void Awake()
    {
        UIManager.UITransform = uiTransform; // UIManager�� ���� ���� UITransform��
                                             // ���� ���� uiTransform�� �Ҵ��մϴ�.
                                             // �̸� ���� UIManager�� ���Ӱ� ������ UI�� �ùٸ��� ��ġ�� �� �ֽ��ϴ�.
        MapManager.MapTransform = mapTransform;

        if (!Main_SceneManager.Instance.isDontDestroy) // Main_SceneManager�� isDontDestroy�� false�� ���
        {
            Main_SceneManager.Instance.ChangeScene("ManagerScene", () => // "ManagerScene" ���� Additive ���� �ε��ϰ�
                                                                         // �ݹ� �Լ� �ȿ��� Ư�� �۾��� �����մϴ�.
                                                                         // ���⼭�� isDontDestroy�� true�� �����ϰ�
                                                                         // �ٽ� "ManagerScene" ���� ��ε��մϴ�.
                                                                         // `()=>`�� ���� ��(Lambda Expression)����,
                                                                         // �Ű������� ���� �͸� �޼��带 �����մϴ�.
                                                                         // { } ������ �ڵ尡 ����� �۾��� ��Ÿ���ϴ�.
                                                                         // �� ���� `Action` Ÿ������ ���޵Ǹ�,
                                                                         // ChangeScene ȣ���� ���� �� ����˴ϴ�.
            {
                Main_SceneManager.Instance.isDontDestroy = true; // isDontDestroy�� true�� �����Ͽ�
                                                                 // DontDestroyOnLoad ���¸� �����մϴ�.
                                                                 // �̴� Ư�� ������Ʈ�� �� ��ȯ �� �������� �ʵ��� �ϱ� �����Դϴ�.
                Main_SceneManager.Instance.UnLoadScene("ManagerScene"); // ManagerScene ���� ��ε��մϴ�.
                                                                        // �޸� ȿ���� ���� �� �̻� �ʿ� ���� �ӽ� ���� �����մϴ�. 
            }, UnityEngine.SceneManagement.LoadSceneMode.Additive); // Additive ���� ���� �ε��մϴ�.
                                                                    // ���� �� ���� �߰������� �ε��Ͽ� ���� �۾��� ������ �� �ֽ��ϴ�.
        }
    }

    protected virtual void OnDestroy()
    {
        uiTransform = null; // uiTransform�� null�� �����Ͽ� �޸� ������ �����մϴ�.
        mapTransform = null;
    }
}
