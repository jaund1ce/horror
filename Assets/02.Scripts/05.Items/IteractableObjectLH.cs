using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteractableObjectLH: MonoBehaviour, IInteractable
{
    [SerializeField]private ObjectData _objectSO;
    private ObjectType _objectType;

    private bool _isOpen = false;

    private void Start()
    {
        _objectType = _objectSO.ObjectType;
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
        switch (_objectType)
        {
            case ObjectType.Door:
                DoorInteracte();
                break;
            case ObjectType.Drawer:
                DrawerInteracte();
                break;
            case ObjectType.Cabinet:
                CabinetInteracte();
                break;
        }
    }

    private void CabinetInteracte()
    {
        throw new NotImplementedException();
    }

    private void DrawerInteracte()
    {
        throw new NotImplementedException();
    }

    private void DoorInteracte()
    {
        throw new NotImplementedException();
    }
}