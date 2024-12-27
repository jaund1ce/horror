using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private bool alreadyApplyForce;
    private bool alreadyAppliedDealing;
    private string attackTransition = "Attack";

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

        LookRotate();

        float normalizeTime = GetNormalizedTime(stateMachine.Enemy.EnemyAnimator, attackTransition);
        if (normalizeTime < 1f)
        {
            if (normalizeTime >= stateMachine.Enemy.Data.ForceTransitionTime)
            {
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

}
