using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UHFPS.Runtime;
using UnityEngine;


public class AttackPoint : MonoBehaviour
{
    private Collider creatureCollider;

    private int damage;

    private List<Collider> alreadyCollider = new List<Collider>();

    private void Awake()
    {
        //## creatureCollider = this.gameObject.GetComponentInParent<Collider>();
    }
    private void OnEnable()
    {
        alreadyCollider.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        //## if (other == creatureCollider) return;
        if (alreadyCollider.Contains(other)) return;

        alreadyCollider.Add(other);

        if (other.TryGetComponent(out Health health)) 
        {
            health.TakeDamage(damage);
        }
    }

    public void SetAttack(int damage) 
    {
        this.damage = damage;
    }
}

