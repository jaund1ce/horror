
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Player.isGround)
        {
            if (stateMachine.MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.WalkState);
                return;
            }
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
