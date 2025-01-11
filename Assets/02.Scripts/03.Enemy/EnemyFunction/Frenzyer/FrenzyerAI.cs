using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyerAI : EnemyAI
{

    private float checkFrenzyTime;
    private float lastFrenzyTime = 30f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        checkFrenzyTime = lastFrenzyTime;
    }

    protected override void Update()
    {
        if (previouseState == AIState.Frenzy)
        {
            checkFrenzyTime = lastFrenzyTime;
        }
        checkFrenzyTime += Time.deltaTime;
        Debug.Log(checkFrenzyTime);
        base.Update();
    }

    protected override void CheckTarget()
    {
        base.CheckTarget();
    }

    protected override void CheckMissTime(bool isTarget)
    {
        base.CheckMissTime(isTarget);
    }

    public override void GetAggroGage(float amount)
    {
        base.GetAggroGage(amount);
    }

    public override void FeelThePlayer()
    {
        base.FeelThePlayer();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override bool IsInAttackRange()
    {
        return base.IsInAttackRange();
    }

    public override int UpdateState()
    {
        if (MainGameManager.Instance.Player.PlayerConditionController.IsDie) return (int)AIState.Idle;

        if (IsAttacking) return (int)EnemyAistate;

        if ((IsAggroGageMax || !IsPlayerMiss) && !IsInAttackRange())
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Data.FeelPlayerRange, playerMask);
            if (colliders.Length == 0 && checkFrenzyTime >= lastFrenzyTime ) 
            {
                checkFrenzyTime = 0f;
                EnemyAistate = AIState.Frenzy;
                return (int)EnemyAistate;
            }
            EnemyAistate = AIState.Chasing;
            return (int)EnemyAistate;
        }
        else if (!IsAggroGageMax && IsPlayerMiss)
        {
            EnemyAistate = AIState.Wandering;
            CheckHalfAggroGage();
            FeelThePlayer();
            return (int)EnemyAistate;
        }
        else if (!IsPlayerMiss && IsInAttackRange())
        {
            checkFrenzyTime = 0f;
            EnemyAistate = AIState.Attacking;
            IsAttacking = true;
            return (int)EnemyAistate;
        }
        else
        {
            EnemyAistate = AIState.Idle;
            return (int)EnemyAistate;
        }
    }
    protected override void PlaySoundBasedOnState()
    {
        base.PlaySoundBasedOnState();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void RealeaseAggroGage(float amount)
    {
        base.RealeaseAggroGage(amount);
    }

    protected override void CheckHalfAggroGage()
    {
        base.CheckHalfAggroGage();
    }
}
