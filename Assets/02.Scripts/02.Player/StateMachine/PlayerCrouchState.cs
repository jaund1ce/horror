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
        base.Update();
        //if (!stateMachine.Player.isGround)//왜 수그리는 순간 바로 채크를 탈출하는가?
        //{
        //    Debug.Log("out?");
        //    stateMachine.ChangeState(stateMachine.FallState);
        //}
    }
}
