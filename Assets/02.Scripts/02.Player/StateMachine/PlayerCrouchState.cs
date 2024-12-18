using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerGroundState
{
    public PlayerCrouchState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.Player.CapsuleCollider.height = 1f;
        stateMachine.MovementSpeedModifier = groundData.CrouchSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    public override void Exit()
    {
        stateMachine.Player.CapsuleCollider.height = 2f;
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.CrouchParameterHash);
    }

    //public override void Update()
    //{
    //    base.Update();
    //    if (!stateMachine.Player.Input.RunningReady)
    //    {
    //        stateMachine.ChangeState(stateMachine.WalkState);
    //    }
    //}
}
