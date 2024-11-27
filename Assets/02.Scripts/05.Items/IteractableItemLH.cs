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
        //UI 표시
    }

    public void HideInteractUI()
    {
        //UI 제거
    }

    public void OnInteract()
    {
        //인벤토리에 적재
    }
}
