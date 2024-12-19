using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class ObjectBase : MonoBehaviour, IInteractable
{
    protected ObjectSO objectSO;

    protected virtual void OnEnable()
    {
        //objectSO = GetComponent<ObjectSO>();    
    }

    public virtual string GetInteractPrompt()
    {
        return null;
    }

    public virtual void OnInteract()
    {

    }

}
