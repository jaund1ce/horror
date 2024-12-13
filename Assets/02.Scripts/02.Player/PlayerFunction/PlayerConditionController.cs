using System;
using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;

public class PlayerConditionController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float PassiveHealth;
    public float Health;
    [SerializeField] private float maxStamina;
    public float PassiveStamina;
    public float Stamina;

    private float staminaAmount = 15f;

    public event Action OnDie;

    void Start()//저장 값을 넣고 싶으면 변경
    {
        Health = maxHealth;
        Stamina = maxStamina;
    }
    private void Update()
    {
        if(Stamina == 0f)
        {
            MainGameManager.Instance.Player.Input.RunningReady = false;
        }
        if (MainGameManager.Instance.Player.Input.isRunning)
        {
            Stamina -= staminaAmount * Time.deltaTime;
        }
        Health = Mathf.Clamp(Health + PassiveHealth*Time.deltaTime, 0, maxHealth);
        Stamina = Mathf.Clamp(Stamina + PassiveStamina * Time.deltaTime, 0, maxStamina);
    }

    public void TakeDamage(int damage)
    {
        if(Health ==0) return;

        Health = Mathf.Max(Health - damage, 0);

        if(Health == 0) OnDie?.Invoke();

        Debug.Log(Health);
    }

    public float GetHPPercentage()
    {
        return Health / maxHealth;
    }

    public float GetStaminaPercentage()
    {
        return Stamina / maxStamina;
    }
}
