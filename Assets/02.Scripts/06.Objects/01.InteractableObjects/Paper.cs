using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paper : ObjectBase
{
    public PaperSO paperData;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override string GetInteractPrompt()
    {
        return "�б�";
    }
    public override void OnInteract()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Animator�� "Interact"��� Ʈ���Ű� �����Ǿ� �־�� ��
        }
        MainGameManager.Instance.paperInteractionCount += paperData.value;
        MainGameManager.Instance.getNewPaper = true;
        //if (MainGameManager.Instance.component == false)
        //{
        //    MainGameManager.Instance.component.enabled = true;
        //}
        Debug.Log($"PaperInteractionCount: {MainGameManager.Instance.paperInteractionCount}");

        UIManager.Instance.Show<PaperUI>();
        
        Destroy(gameObject);
    }

}
