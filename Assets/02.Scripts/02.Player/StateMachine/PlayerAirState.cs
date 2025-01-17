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
        base.Enter();
        ChangePlayerStateEnter();
        StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        ChangePlayerStateExit();
        StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void ChangePlayerStateEnter()
    {
        stateMachine.isAir = true;
    }

    private void ChangePlayerStateExit()
    {
        stateMachine.isAir = false;
    }
}
