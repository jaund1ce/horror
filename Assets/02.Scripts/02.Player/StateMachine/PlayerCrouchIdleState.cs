using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerCrouchState
{
    public PlayerCrouchIdleState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.CrouchingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.CrouchingParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!MainGameManager.Instance.Player.isHiding && !stateMachine.Player.Input.Crouching)//hide 상태에서는 예외적으로 탈출 X
        {
            stateMachine.ChangeState(stateMachine.IdleState); 
        }
    }
}
