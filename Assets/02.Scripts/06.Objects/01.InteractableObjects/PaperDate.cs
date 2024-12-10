using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paper", menuName = "New Paper")]
public class PaperDate : ScriptableObject
{
    public string Name;
    public string description;
    public GameObject dropPrefab;
    public int value;
}
