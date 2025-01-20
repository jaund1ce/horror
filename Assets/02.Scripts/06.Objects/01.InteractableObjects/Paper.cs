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
        MainGameManager.Instance.PaperInteraction.Add(PaperID);
        MainGameManager.Instance.getNewPaper = true;
        
        for (int i = 0; i < MainGameManager.Instance.PaperInteraction.Count; i++) 
        {
            Debug.Log($"PaperInteractionCount: {MainGameManager.Instance.PaperInteraction[i]}");
        }
        
        SoundManger.Instance.MakeEnviornmentSound("PaperSound");
        UIManager.Instance.Show<PaperUI>();
        
        Destroy(gameObject);
    }

}
