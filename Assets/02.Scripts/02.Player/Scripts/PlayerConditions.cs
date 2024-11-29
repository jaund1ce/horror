using System;
using UnityEngine;


public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition aggro { get { return uiCondition.aggro; } }

    public event Action onTakeDamage;

    private void Update()
    {
        aggro.Subtract(aggro.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (aggro.curValue > 99.9f)
        {
            //exposed to the enemy
        }

        if (health.curValue < 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Attention(float amount)
    {
        aggro.Add(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakeDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}