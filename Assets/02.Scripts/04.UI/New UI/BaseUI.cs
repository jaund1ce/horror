using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    internal Canvas canvas;

    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
