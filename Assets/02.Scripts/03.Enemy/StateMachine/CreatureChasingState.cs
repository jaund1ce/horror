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
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Creature.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Creature.AnimationData.WalkParameterHash);
        
        
    }

    public override void Update()
    {
        base.Update();

        if (!IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            Debug.Log("µé¾î¿È ?");
            return;
        }
        else if (IsInAttackRange()) 
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }

    protected bool IsInAttackRange() 
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Creature.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Creature.Data.AttackRange * stateMachine.Creature.Data.AttackRange;
    }
}
