using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        MovementSpeedModifier = groundData.RunSpeedModifier;
        StartAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        MovementSpeedModifier = groundData.WalkSpeedModifier;
        StopAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
        
        
    }

    public override void Update()
    {
        base.Update();
    }

}
