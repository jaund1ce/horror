using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class ObjectBase : MonoBehaviour, IInteractable
{
    public ObjectSO ObjectSO;

    protected virtual void OnEnable()
    {
    }

    public abstract string GetInteractPrompt();

    public abstract void OnInteract();
}
