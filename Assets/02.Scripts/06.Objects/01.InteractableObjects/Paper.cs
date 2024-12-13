using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paper : MonoBehaviour, IInteractable
{
    public PaperSO paperData;

    public string GetInteractPrompt()
    {
        return "�б�";
    }
    public void OnInteract()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Animator�� "Interact"��� Ʈ���Ű� �����Ǿ� �־�� ��
        }
        UIManager.Instance.paperInteractionCount += paperData.value;
        Debug.Log($"PaperInteractionCount: {UIManager.Instance.paperInteractionCount}");

        UIManager.Instance.Show<PaperUI>();
        
        Destroy(gameObject);
    }

}
