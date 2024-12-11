using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paper", menuName = "New Paper")]
public class PaperSO : ScriptableObject
{
    public string Name;
    public string description;
    public GameObject dropPrefab;
    public int value;
}
