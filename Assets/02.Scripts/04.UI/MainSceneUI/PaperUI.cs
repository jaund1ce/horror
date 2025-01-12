using UnityEngine;

public class PaperUI : PopupUI
{
    [SerializeField] private GameObject paperButton1; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail1; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton2; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail2; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton3; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail3; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton4; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail4; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton5; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail5; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton6; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail6; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton7; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail7; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton8; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail8; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton9; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail9; // PaperDetail_1 오브젝트
    [SerializeField] private GameObject paperButton10; // PaperButton_1 오브젝트
    [SerializeField] private GameObject paperDetail10; // PaperDetail_1 오브젝트
    public GameObject targetObject;

    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    // 필요하다면 추가적인 동작 구현
    public override void OpenUI()
    { 
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    private void Start()
    {
        if (MainGameManager.Instance.paperInteractionCount >= 1)
        {
            if (paperButton1 != null)
            {
                paperButton1.SetActive(true);
            }

            if (paperDetail1 != null)
            {
                paperDetail1.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 2)
        {
            if (paperButton2 != null)
            {
                paperButton2.SetActive(true);
            }

            if (paperDetail2 != null)
            {
                paperDetail2.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 3)
        {
            if (paperButton3 != null)
            {
                paperButton3.SetActive(true);
            }

            if (paperDetail3 != null)
            {
                paperDetail3.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 4)
        {
            if (paperButton4 != null)
            {
                paperButton4.SetActive(true);
            }

            if (paperDetail4 != null)
            {
                paperDetail4.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 5)
        {
            if (paperButton5 != null)
            {
                paperButton5.SetActive(true);
            }

            if (paperDetail5 != null)
            {
                paperDetail5.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 6)
        {
            if (paperButton6 != null)
            {
                paperButton6.SetActive(true);
            }

            if (paperDetail6 != null)
            {
                paperDetail6.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 7)
        {
            if (paperButton7 != null)
            {
                paperButton7.SetActive(true);
            }

            if (paperDetail7 != null)
            {
                paperDetail7.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 8)
        {
            if (paperButton8 != null)
            {
                paperButton8.SetActive(true);
            }

            if (paperDetail8 != null)
            {
                paperDetail8.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 9)
        {
            if (paperButton9 != null)
            {
                paperButton9.SetActive(true);
            }

            if (paperDetail9 != null)
            {
                paperDetail9.SetActive(true);
            }
        }
        if (MainGameManager.Instance.paperInteractionCount >= 10)
        {
            if (paperButton10 != null)
            {
                paperButton10.SetActive(true);
            }

            if (paperDetail10 != null)
            {
                paperDetail10.SetActive(true);
            }
        }
    }

    public void OpenBigUI()
    {
        if (targetObject != null) // targetObject가 null이 아니라면(Destroy, 아예 오브젝트가 없지 않다면)
        {
            bool isActive = targetObject.activeSelf; // activeSelf = targetObject의 SetAtive상태의(True, False) bool 지역 변수 
            targetObject.SetActive(!isActive); // activeSelf(targetObject의 SetAtive상태(True, False) )의 반대로 SetActive 한다
        }
    }
}
