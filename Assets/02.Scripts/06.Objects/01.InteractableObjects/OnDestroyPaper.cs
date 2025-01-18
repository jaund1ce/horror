using UnityEngine;
using System;

public class OnDestroyPaper : MonoBehaviour
{
    // �ı� �� ����� �׼�
    public static Action<GameObject> OnObjectDestroyed;

    private void OnDestroy()
    {
        // ������Ʈ�� �ı��Ǿ����� �˸��� �ڽ��� ����
        OnObjectDestroyed?.Invoke(gameObject);
    }
}
