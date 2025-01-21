using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public GameObject[] documentObjects; // 문서 이미지가 담긴 오브젝트 배열
    public Button leftButton;  // 왼쪽으로 넘기는 버튼
    public Button rightButton; // 오른쪽으로 넘기는 버튼
    [SerializeField] public Slider progressBar; // 진행률 표시를 위한 Slider UI
    [SerializeField] private Button confirmButton;
    private int currentIndex = 0; // 현재 활성화된 오브젝트 인덱스

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);

        // 초기 상태 설정: 첫 번째 오브젝트만 활성화
        UpdateActiveObject();
    }

    // 왼쪽 버튼 클릭 시 호출
    void MoveLeft()
    {
        currentIndex = (currentIndex - 1 + documentObjects.Length) % documentObjects.Length; // 인덱스 감소, 순환 처리
        UpdateActiveObject();
    }

    // 오른쪽 버튼 클릭 시 호출
    void MoveRight()
    {
        currentIndex = (currentIndex + 1) % documentObjects.Length; // 인덱스 증가, 순환 처리
        UpdateActiveObject();
    }

    // 활성화된 오브젝트 업데이트
    void UpdateActiveObject()
    {
        for (int i = 0; i < documentObjects.Length; i++)
        {
            documentObjects[i].SetActive(i == currentIndex); // 현재 인덱스만 활성화
        }
    }

    public void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = Mathf.Clamp01(progress); // 진행률 업데이트
        }

    }

    public void SetConfirmButtonActive(bool isActive)
    {
        if (confirmButton != null)
        {
            confirmButton.gameObject.SetActive(isActive); // 버튼 활성화/비활성화
        }
    }

}
 