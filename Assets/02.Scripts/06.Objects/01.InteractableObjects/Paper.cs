using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paper : ObjectBase
{
    public int PaperID;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override string GetInteractPrompt()
    {
        return $"{PaperID}";
    }
    public override void OnInteract()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Animator�� "Interact"��� Ʈ���Ű� �����Ǿ� �־�� ��
        }
        MainGameManager.Instance.PaperInteraction.Add(PaperID);
        MainGameManager.Instance.getNewPaper = true;
        Debug.Log($"PaperInteractionCount: {MainGameManager.Instance.PaperInteraction}");
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        UIManager.Instance.Show<PaperUI>();
        
        Destroy(gameObject);
    }

}
