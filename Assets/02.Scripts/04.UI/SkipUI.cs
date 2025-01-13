using UnityEngine;


public class SkipUI : BaseUI
{

    private Animator animator;

    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    void Start()
    {
        // 오브젝트의 Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>();

        // Animator가 없으면 경고를 출력하고 이 스크립트를 비활성화합니다.
        if (animator == null)
        {
            Debug.LogWarning($"Animator component is missing on {gameObject.name}.");
            enabled = false;
        }
    }

    void Update()
    {
        if (animator != null)
        {
            // 현재 활성화된 애니메이션 상태를 가져옵니다.
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 애니메이션이 종료되었는지 확인합니다.
            if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
            {
                Destroy(gameObject); // 애니메이션 종료 시 오브젝트를 파괴합니다.
            }
        }
    }
}
