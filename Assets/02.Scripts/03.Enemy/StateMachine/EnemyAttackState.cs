using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    bool alreadyApplyForce;
    bool alreadyAppliedDealing;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MovementSpeedModifier = 0f;
        StartAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);

        alreadyApplyForce = false;
        alreadyAppliedDealing = false;
    }

    public override void Exit()
    {
        base.Exit();
        MovementSpeedModifier = groundData.WalkSpeedModifier;
        StopAnimation(stateMachine.Enemy.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.BaseAttackParameterHash);
        
        
    }

    public override void Update()
    {
        base.Update();

        //ForceMove();

        float normalizeTime = GetNormalizedTime(stateMachine.Enemy.CreatureAnimator, "Attack");
        if (normalizeTime < 1f)
        {
            if (normalizeTime >= stateMachine.Enemy.Data.ForceTransitionTime)
            {
                TryApplyForce();
            }

            //공격 활성화 시간 컨트롤
            if (!alreadyAppliedDealing && normalizeTime >= stateMachine.Enemy.Data.Dealing_Start_TransitionTime) 
            {
                stateMachine.Enemy.AttackPoint.SetAttack(stateMachine.Enemy.Data.Damage);
                stateMachine.Enemy.AttackPoint.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && normalizeTime >= stateMachine.Enemy.Data.Dealing_End_TransitionTime) 
            {
                stateMachine.Enemy.EnemyAI.IsAttacking = false;
                stateMachine.Enemy.AttackPoint.gameObject.SetActive(false);
            }

        }
    }

    private void TryApplyForce() 
    {
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        stateMachine.Enemy.ForceReceiver.Reset();

        stateMachine.Enemy.ForceReceiver.AddForce(Vector3.forward * stateMachine.Enemy.Data.Force);
    }
}
