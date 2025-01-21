using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class BaseMap : MonoBehaviour
{

    public virtual void OpenMap()
    { }
    public virtual void CloseMap()
    {
        MapManager.Instance.HideMap(gameObject.name);  
    }
}
  