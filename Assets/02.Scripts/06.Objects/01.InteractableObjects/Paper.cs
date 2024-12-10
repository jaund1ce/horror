using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Paper : MonoBehaviour, IInteractable
{
    public PaperDate paperData;

    public string GetInteractPrompt()
    {
        string str = $"{paperData.Name}\n{paperData.description}";
        return "";
    }
    public void OnInteract()
    {
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            // PaperInteractionCount 업데이트
            playerInteraction.paperInteractionCount += paperData.value;

            Debug.Log($"PaperInteractionCount: {playerInteraction.paperInteractionCount}");
            UIManager.Instance.Show<PaperUI>();
            // PaperInteractionCount에 따라 특정 동작 수행
            if (playerInteraction.paperInteractionCount == 1)
            {
               
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
