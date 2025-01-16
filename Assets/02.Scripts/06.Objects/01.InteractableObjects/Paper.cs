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
        return "Read";
    }
    public override void OnInteract()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Animator에 "Interact"라는 트리거가 설정되어 있어야 함
        }
        MainGameManager.Instance.paperInteractionCount += 1;
        MainGameManager.Instance.getNewPaper = true;
        Debug.Log($"PaperInteractionCount: {MainGameManager.Instance.paperInteractionCount}");
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        UIManager.Instance.Show<PaperUI>();
        
        Destroy(gameObject);
    }

}
