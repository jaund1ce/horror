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
    public EquipItemBase CurEquip;
    public Transform EquipPoint;

    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        CurEquip = Instantiate(data.itemSO.EquipPrefab, EquipPoint).GetComponent<EquipItemBase>();


    }

    public void UnEquip()
    {
        if (CurEquip != null)
        {
            Destroy(CurEquip.gameObject);
            CurEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CurEquip != null)
        {
            CurEquip.OnUseInput();
        }
    }

}

