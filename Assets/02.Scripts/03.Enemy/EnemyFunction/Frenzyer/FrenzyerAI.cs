using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyerAI : EnemyAI
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
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
        if (IsAttacking) return (int)EnemyAistate;

        if ((IsAggroGageMax || !isPlayerMiss) && !IsInAttackRange())
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Data.FeelPlayerRange, playerMask);
            if (colliders.Length == 0) 
            {
                EnemyAistate = AIState.Frenzy;
                return (int)EnemyAistate;
            }
            EnemyAistate = AIState.Chasing;
            return (int)EnemyAistate;
        }
        else if (!IsAggroGageMax && isPlayerMiss)
        {
            EnemyAistate = AIState.Wandering;
            FeelThePlayer();
            return (int)EnemyAistate;
        }
        else if (!isPlayerMiss && IsInAttackRange())
        {
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

}
