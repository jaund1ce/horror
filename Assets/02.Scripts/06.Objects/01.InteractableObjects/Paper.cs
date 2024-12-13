using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paper : MonoBehaviour, IInteractable
{
    public PaperSO paperData;

    public string GetInteractPrompt()
    {
        string str = $"읽기";
        return "";
    }
    public void OnInteract()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Animator에 "Interact"라는 트리거가 설정되어 있어야 함
        }
        UIManager.Instance.paperInteractionCount += paperData.value;
        Debug.Log($"PaperInteractionCount: {UIManager.Instance.paperInteractionCount}");

        UIManager.Instance.Show<PaperUI>();
        
        Destroy(gameObject);
    }

}
