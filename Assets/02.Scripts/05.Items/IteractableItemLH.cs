using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteractableItemLH : MonoBehaviour, IInteractable
{
    [SerializeField]private ItemData _itemSO;
    private ItemType _itemType;

    private void Start()
    {
        _itemType = _itemSO.ItemType;
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
        //�κ��丮�� ����
    }
}
