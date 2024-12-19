using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("CrouchState in");
        base.Enter();
        stateMachine.Player.CapsuleCollider.height = 1f;
        stateMachine.MovementSpeedModifier = groundData.CrouchSpeedModifier;
        StartAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.CapsuleCollider.height = 2f;
        Debug.Log("CrouchState out");
        StopAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Update()
    {
        if (!stateMachine.Player.isGround)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }
}
