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
        // ������Ʈ�� Animator ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator>();

        // Animator�� ������ ��� ����ϰ� �� ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�.
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
            // ���� Ȱ��ȭ�� �ִϸ��̼� ���¸� �����ɴϴ�.
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // �ִϸ��̼��� ����Ǿ����� Ȯ���մϴ�.
            if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
            {
                Destroy(gameObject); // �ִϸ��̼� ���� �� ������Ʈ�� �ı��մϴ�.
            }
        }
    }
}
