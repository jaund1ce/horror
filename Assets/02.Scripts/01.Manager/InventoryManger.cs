using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    private ItemSO LastSelectItemData;
    public ItemSO CurrentSelectItemData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeInventory(ItemSO itemData)
    {
        CurrentSelectItemData = itemData;

        if (CurrentSelectItemData == LastSelectItemData) return;

        LastSelectItemData = CurrentSelectItemData;
    }
}
