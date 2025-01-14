using UnityEngine;

public class MagnifierCamera : MonoBehaviour
{
    public Camera magnifierCamera; // 돋보기 역할을 할 카메라
    public float magnification = 2.0f; // 확대 배율
    public float magnifierSize = 0.2f; // 돋보기 크기 (Viewport Rect 크기)

    void Start()
    {
        if (magnifierCamera != null)
        {
            // 확대 설정
            magnifierCamera.orthographic = true;
            magnifierCamera.orthographicSize = Camera.main.orthographicSize / magnification;

            // Viewport Rect 설정 (화면 상에서 작은 영역만 차지)
            magnifierCamera.rect = new Rect(0.4f, 0.4f, magnifierSize, magnifierSize);
        }
    }

    void Update()
    {
        if (magnifierCamera == null) return;

        // 마우스 위치를 따라 돋보기 카메라 이동
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 돋보기 카메라 위치 설정
        magnifierCamera.transform.position = new Vector3(worldPos.x, worldPos.y, -10f);
    }
}
