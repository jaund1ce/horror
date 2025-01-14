using System;
using UnityEngine;
using System.Threading;

public class PlayerConditionController : MonoBehaviour
{
    public event Action OnDie;
    [HideInInspector]public bool IsDie;
    [SerializeField] private float maxHealth;
    public float PassiveHealth;
    public float Health;
    [SerializeField] private float maxStamina;
    public float PassiveStamina;
    public float Stamina;
    [SerializeField] private float staminaUseAmount = 15f;

    private PlayerBreatheType playerBreatheType = PlayerBreatheType.Normal;
    private Player player;

    void Start()//저장 값을 넣고 싶으면 변경
    {
        Health = maxHealth;
        Stamina = maxStamina;

        player = MainGameManager.Instance.Player;
        IsDie = false;
    }
    private void Update()
    {
        if (Stamina == 0f) player.Input.RunningReady = false;

        if (player.StateMachine.isRunning)
        {
            Stamina -= staminaUseAmount * Time.deltaTime;
        }

        RecoverConditions();
        
        if (ChangeStaminaState(GetStaminaPercentage()))
        {
            SoundManger.Instance.ChangeBreatheBeatSound(playerBreatheType);
        }
    }

    private void RecoverConditions()
    {
        if (player.isHiding)
        {
            Health = Mathf.Clamp(Health + PassiveHealth * Time.deltaTime, 0, maxHealth);
            player.OnHPChange();
        }

        if (player.StateMachine.isCrouching)
        {
            Stamina += PassiveStamina * Time.deltaTime;
        }

        Stamina = Mathf.Clamp(Stamina + PassiveStamina * Time.deltaTime, 0, maxStamina);
    }

    public void TakeDamage(int damage, EnemyAI enemy)
    {
        EnemyAI attackEnemy = enemy;
        player.OnHPChange();

        if (IsDie) return;

        Health = Mathf.Max(Health - damage, 0);
        SoundManger.Instance.ChangeBreatheBeatSound(PlayerBreatheType.Damaged);

        if (Health == 0) 
        {
            IsDie = true;
            PlayerDie(attackEnemy);
        }
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
        player.OnHPChange();
    }

    private bool ChangeStaminaState(float staminaPercentage)
    {
        if (staminaPercentage > 0.7f && playerBreatheType != PlayerBreatheType.Normal)
        {
            playerBreatheType = PlayerBreatheType.Normal;
            return true;
        }
        else if (staminaPercentage <= 0.7f && staminaPercentage > 0.5f && playerBreatheType != PlayerBreatheType.Tired)
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

    public void PlayerDie(EnemyAI enemyAI)
    {
        EnemyAI enemy = enemyAI;
        SoundState enemyKillSound = enemy.soundStates[AIState.Chasing];
        SoundManger.Instance.Enviroment.PlayOneShot(enemyKillSound.Sound);
        Transform firstChild = enemy.transform.GetChild(0);
        firstChild.gameObject.SetActive(true);
        OnDie?.Invoke();
    }
}
