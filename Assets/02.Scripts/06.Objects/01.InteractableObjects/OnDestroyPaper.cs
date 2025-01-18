using UnityEngine;
using System;

public class OnDestroyPaper : MonoBehaviour
{
    // 파괴 시 실행될 액션
    public static Action<GameObject> OnObjectDestroyed;

    private void OnDestroy()
    {
        // 오브젝트가 파괴되었음을 알리고 자신을 전달
        OnObjectDestroyed?.Invoke(gameObject);
    }
}
