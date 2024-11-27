using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteractableDocumentLH : MonoBehaviour, IInteractable
{
    [SerializeField]private DocumentData _documentSO;
    private int _documentNum;

    private void Start()
    {
        _documentNum = _documentSO.Number;
    }
    public void ShowInteractUI()
    {
        //UI 표시
    }

    public void HideInteractUI()
    {
        //UI 제거
    }

    public void OnInteract()
    {
        //문서함에 추가에 적재
    }
}
