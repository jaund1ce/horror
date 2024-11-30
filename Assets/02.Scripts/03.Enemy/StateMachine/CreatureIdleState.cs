using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIdleState : CreatureBaseState
{
    public CreatureIdleState(CreatureStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Creature.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Creature.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (IsInChasingRange()) 
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
    }
}
