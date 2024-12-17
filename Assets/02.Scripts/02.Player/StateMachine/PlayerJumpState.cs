using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Player.PlayerRigidbody.AddForce(Vector3.up * 40f, ForceMode.Impulse);//jumpforce 라는 숫자로 바꿔야 함
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.Player.PlayerRigidbody.velocity.y < 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
