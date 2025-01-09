using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class Equipment : MonoBehaviour
{
    public Transform EquipPoint;
    [HideInInspector]public EquipItemBase CurEquip;

    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void EquipNew(InventoryData data)
    {
        UnEquip();
        CurEquip = Instantiate(data.ItemData.itemSO.EquipPrefab, EquipPoint).GetComponent<EquipItemBase>();
        CurEquip.gameObject.transform.localPosition = data.ItemData.itemSO.EquipPrefab.transform.position;
        CurEquip.gameObject.transform.localRotation = data.ItemData.itemSO.EquipPrefab.transform.rotation;
    }

    public void UnEquip()
    {
        if (CurEquip != null)
        {
            if (CurEquip is EquipLight && CurEquip.OnUsing){ CurEquip.OnUse(); }

            Destroy(CurEquip.gameObject);
            CurEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && CurEquip != null)
        {
            CurEquip.OnUseInput();
        }
    }

}

