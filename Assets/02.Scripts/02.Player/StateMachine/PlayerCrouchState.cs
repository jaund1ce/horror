using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.isCrouching = true;
        stateMachine.Player.CapsuleCollider.height = 0.8f;
        stateMachine.Player.CapsuleCollider.radius = 0.35f;
        stateMachine.Player.CapsuleCollider.center = new Vector3(0,0.4f,0);
        stateMachine.MovementSpeedModifier = groundData.CrouchSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.isCrouching = false;
        stateMachine.Player.CapsuleCollider.height = 2f;
        stateMachine.Player.CapsuleCollider.radius = 0.25f;
        stateMachine.Player.CapsuleCollider.center = new Vector3(0, 1f, 0);
        StopAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Update()
    {
        if (!stateMachine.Player.Input.isCrouching && !stateMachine.Player.isHiding)
        {
            stateMachine.ChangeState(stateMachine.IdleState);            
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.isHiding)
        {
            return;
        }

        base.OnMovementCanceled(context);
    }
}
