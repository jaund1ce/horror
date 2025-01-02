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

    [SerializeField] private float staminaUseAmount = 15f;

    public event Action OnDie;
    private PlayerBreatheType playerBreatheType = PlayerBreatheType.Normal;

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
            Stamina -= staminaUseAmount * Time.deltaTime;
        }
        else if (MainGameManager.Instance.Player.Input.isCrouching)
        {
            Stamina += PassiveStamina * Time.deltaTime;
        }

        Health = Mathf.Clamp(Health + PassiveHealth*Time.deltaTime, 0, maxHealth);
        Stamina = Mathf.Clamp(Stamina + PassiveStamina * Time.deltaTime, 0, maxStamina);
        if (ChangeState(GetStaminaPercentage()))
        {
            SoundManger.Instance.ChangeBreatheBeatSound(playerBreatheType);
        }
    }

    public void TakeDamage(int damage)
    {
        if(Health ==0) return;

        Health = Mathf.Max(Health - damage, 0);

        if(Health == 0) OnDie?.Invoke();
    }

    public float GetHPPercentage()
    {
        return Health / maxHealth;
    }

    public float GetStaminaPercentage()
    {
        return Stamina / maxStamina;
    }

    public void AddHealth(int amount) 
    {
        Health = Mathf.Min(Health + amount, maxHealth);
    }

    private bool ChangeState(float staminaPercentage)
    {
        if (staminaPercentage > 0.8f && playerBreatheType != PlayerBreatheType.Normal)
        {
            playerBreatheType = PlayerBreatheType.Normal;
            return true;
        }
        else if (staminaPercentage <= 0.8f && staminaPercentage > 0.5f && playerBreatheType != PlayerBreatheType.Tired)
        {
            playerBreatheType = PlayerBreatheType.Tired;
            return true;
        }
        else if (staminaPercentage <= 0.5f && staminaPercentage > 0.2f && playerBreatheType != PlayerBreatheType.Exhausted)
        {
            playerBreatheType = PlayerBreatheType.Exhausted;
            return true;
        }
        else if (staminaPercentage <= 0.2f && staminaPercentage > 0f && playerBreatheType != PlayerBreatheType.Fatigued)
        {
            playerBreatheType = PlayerBreatheType.Fatigued;
            return true;
        }
        else return false;
    }
}
