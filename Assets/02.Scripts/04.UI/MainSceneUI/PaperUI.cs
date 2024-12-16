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

    }
}
