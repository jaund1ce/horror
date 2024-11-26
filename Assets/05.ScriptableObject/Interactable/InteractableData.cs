using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class InteractableData : ScriptableObject
{
    [Header("Interactable Data")]
    public InteractableType InteractableType;
    public string InteractableName;
    public string InteractablePrompt;
}
