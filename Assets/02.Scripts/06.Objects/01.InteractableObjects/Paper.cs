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
        return "읽기";
    }
    public override void OnInteract()
    {
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Animator에 "Interact"라는 트리거가 설정되어 있어야 함
        }
        MainGameManager.Instance.paperInteractionCount += paperData.value;
        Debug.Log($"PaperInteractionCount: {MainGameManager.Instance.paperInteractionCount}");

        UIManager.Instance.Show<PaperUI>();
        SoundManger.Instance.MakeEnviormentSound("PaperSound");
        
        Destroy(gameObject);
    }

}
