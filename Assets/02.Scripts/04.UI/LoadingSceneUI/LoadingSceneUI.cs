using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public GameObject[] documentObjects; // ���� �̹����� ��� ������Ʈ �迭
    public Button leftButton;  // �������� �ѱ�� ��ư
    public Button rightButton; // ���������� �ѱ�� ��ư
    [SerializeField] public Slider progressBar; // ����� ǥ�ø� ���� Slider UI
    [SerializeField] private Button confirmButton;
    private int currentIndex = 0; // ���� Ȱ��ȭ�� ������Ʈ �ε���

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);

        // �ʱ� ���� ����: ù ��° ������Ʈ�� Ȱ��ȭ
        UpdateActiveObject();
    }

    // ���� ��ư Ŭ�� �� ȣ��
    void MoveLeft()
    {
        currentIndex = (currentIndex - 1 + documentObjects.Length) % documentObjects.Length; // �ε��� ����, ��ȯ ó��
        UpdateActiveObject();
    }

    // ������ ��ư Ŭ�� �� ȣ��
    void MoveRight()
    {
        currentIndex = (currentIndex + 1) % documentObjects.Length; // �ε��� ����, ��ȯ ó��
        UpdateActiveObject();
    }

    // Ȱ��ȭ�� ������Ʈ ������Ʈ
    void UpdateActiveObject()
    {
        for (int i = 0; i < documentObjects.Length; i++)
        {
            documentObjects[i].SetActive(i == currentIndex); // ���� �ε����� Ȱ��ȭ
        }
    }

    public void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = Mathf.Clamp01(progress); // ����� ������Ʈ
        }

    }

    public void SetConfirmButtonActive(bool isActive)
    {
        if (confirmButton != null)
        {
            confirmButton.gameObject.SetActive(isActive); // ��ư Ȱ��ȭ/��Ȱ��ȭ
        }
    }

}
 