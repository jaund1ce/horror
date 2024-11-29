using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInfo : MonoBehaviour
{
    public string Name;
    public List<ItemData> Inventory = new List<ItemData>();
}
