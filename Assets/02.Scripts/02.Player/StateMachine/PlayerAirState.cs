using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.isAir = true;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Exit()
    {
        stateMachine.isAir = false;
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
