using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EnemyWanderState : EnemyBaseState
{
    public EnemyWanderState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MovementSpeedModifier = groundData.WalkSpeedModifier;
        StartAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.WalkParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.WalkParameterHash);

    }

    public override void Update()
    {
        base.Update();
    }
}

