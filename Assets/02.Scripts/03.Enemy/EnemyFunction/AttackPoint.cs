using System.Collections.Generic;
using UnityEngine;


public class AttackPoint : MonoBehaviour
{
    private Collider creatureCollider;
    private EnemyAI enemy;

    private int damage;

    private List<Collider> alreadyCollider = new List<Collider>();

    private void Awake()
    {
        //## creatureCollider = this.gameObject.GetComponentInParent<Collider>();
        enemy = GetComponentInParent<EnemyAI>();
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

        if (other.TryGetComponent(out PlayerConditionController health)) 
        {
            health.TakeDamage(damage, enemy);
        }
    }

    public void SetAttack(int damage) 
    {
        this.damage = damage;
    }
}

