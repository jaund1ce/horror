using UnityEngine;

public class MagnifierCamera : MonoBehaviour
{
    public Camera magnifierCamera; // ������ ������ �� ī�޶�
    public float magnification = 2.0f; // Ȯ�� ����
    public float magnifierSize = 0.2f; // ������ ũ�� (Viewport Rect ũ��)

    void Start()
    {
        if (magnifierCamera != null)
        {
            // Ȯ�� ����
            magnifierCamera.orthographic = true;
            magnifierCamera.orthographicSize = Camera.main.orthographicSize / magnification;

            // Viewport Rect ���� (ȭ�� �󿡼� ���� ������ ����)
            magnifierCamera.rect = new Rect(0.4f, 0.4f, magnifierSize, magnifierSize);
        }
    }

    void Update()
    {
        if (magnifierCamera == null) return;

        // ���콺 ��ġ�� ���� ������ ī�޶� �̵�
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // ������ ī�޶� ��ġ ����
        magnifierCamera.transform.position = new Vector3(worldPos.x, worldPos.y, -10f);
    }
}
