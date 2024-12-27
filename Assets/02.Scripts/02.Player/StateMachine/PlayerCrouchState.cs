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
        stateMachine.Player.CapsuleCollider.height = 0.8f;
        stateMachine.Player.CapsuleCollider.radius = 0.35f;
        stateMachine.Player.CapsuleCollider.center = new Vector3(0,0.4f,0);
        stateMachine.MovementSpeedModifier = groundData.CrouchSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.CapsuleCollider.height = 2f;
        stateMachine.Player.CapsuleCollider.radius = 0.25f;
        stateMachine.Player.CapsuleCollider.center = new Vector3(0, 1f, 0);
        StopAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Update()
    {
        base.Update(); //crouch 상태에서도 이동이 가능

        if (!stateMachine.isCrouching && !stateMachine.Player.isHiding)
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
