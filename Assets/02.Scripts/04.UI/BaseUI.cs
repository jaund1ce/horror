using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class BaseUI : MonoBehaviour
{
    internal Canvas canvas;

    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
    }
    public virtual void CloseUI()
    {
        //UIManager.Instance.Hide(this.GetType());
         UIManager.Instance.Hide(gameObject.name);  // UIManager�� ���� ���� UI�� ��Ȱ��ȭ�մϴ�.
                                                    // �̸� ���� UI�� �޸𸮿� ȭ�� �ڿ��� ȿ�������� �����մϴ�.
    }
}
