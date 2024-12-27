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
         UIManager.Instance.Hide(gameObject.name);  // UIManager를 통해 현재 UI를 비활성화합니다.
                                                    // 이를 통해 UI의 메모리와 화면 자원을 효율적으로 관리합니다.
    }
}
