using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GrondParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GrondParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(!stateMachine.Player.isGround)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return;

        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected override void OnCrouchStarted(InputAction.CallbackContext context)
    {
        base.OnCrouchStarted(context);
        stateMachine.ChangeState(stateMachine.CrouchIdleState);
    }
}
