using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
        
        
        
    }

    public override void Update()
    {
        base.Update();
        //stateMachine.Enemy.EnemyAI.EnemyAistate = AIState.Wandering;
    }
}
