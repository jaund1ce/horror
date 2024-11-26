using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteractableLH : MonoBehaviour, IInteractable
{
    [SerializeField]private InteractableData _interactableSO;

    private void Start()
    {
        if(_interactableSO.InteractableType == InteractableType.None)
        {
            Debug.LogError("Index Error.");
        }
    }

    public void HideInteractUI()
    {
        //UI 제거
    }

    public void OnInteract()
    {
        
    }

    public void ShowInteractUI()
    {
        //UI 표시
    }

    
}
