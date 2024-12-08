using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class CreatureAttackState : CreatureBaseState
{
    bool alreadyApplyForce;

    public CreatureAttackState(CreatureStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MovementSpeedModifier = 0f;
        StartAnimation(stateMachine.Creature.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Creature.AnimationData.BaseAttackParameterHash);

        alreadyApplyForce = false;
    }

    public override void Exit()
    {
        base.Exit();
        MovementSpeedModifier = groundData.WalkSpeedModifier;
        StopAnimation(stateMachine.Creature.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Creature.AnimationData.BaseAttackParameterHash);
        
        
    }

    public override void Update()
    {
        base.Update();

        //ForceMove();

        float normalizeTime = GetNormalizedTime(stateMachine.Creature.CreatureAnimator, "Attack");
        if (normalizeTime < 1f)
        {
            if (normalizeTime >= stateMachine.Creature.Data.ForceTransitionTime)
            {
                TryApplyForce();
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private void TryApplyForce() 
    {
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        stateMachine.Creature.ForceReceiver.Reset();

        stateMachine.Creature.ForceReceiver.AddForce(Vector3.forward * stateMachine.Creature.Data.Force);
    }
}
