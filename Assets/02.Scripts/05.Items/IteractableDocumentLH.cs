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
        //UI ǥ��
    }

    public void HideInteractUI()
    {
        //UI ����
    }

    public void OnInteract()
    {
        //�����Կ� �߰��� ����
    }
}
