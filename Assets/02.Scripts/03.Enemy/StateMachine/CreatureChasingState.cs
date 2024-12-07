using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureChasingState : CreatureBaseState
{
    public CreatureChasingState(CreatureStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        MovementSpeedModifier = groundData.RunSpeedModifier;
        StartAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Creature.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        MovementSpeedModifier = groundData.WalkSpeedModifier;
        StopAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Creature.AnimationData.RunParameterHash);
        
        
    }

    public override void Update()
    {
        base.Update();

        /*if (stateMachine.Creature.CreatureAI.CreatureAistate != AIState.Chasing *//*!IsInChasingRange()*//*)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        else*/ if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
        else if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Wandering) 
        {
            stateMachine.ChangeState(stateMachine.WanderState);
            return;
        }
    }

    protected bool IsInAttackRange() 
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Creature.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Creature.Data.AttackRange * stateMachine.Creature.Data.AttackRange;
    }
}
