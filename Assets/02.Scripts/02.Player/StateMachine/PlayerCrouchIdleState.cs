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
        Debug.Log("Crouch idle");
        StartAnimation(stateMachine.Player.AnimationData.CrouchingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("C I S");
        StopAnimation(stateMachine.Player.AnimationData.CrouchingParameterHash);
    }

    public override void Update()
    {
        //base.Update();
        if (MainGameManager.Instance.Player.Input.Crouching)
        {
            stateMachine.ChangeState(this); 
        }
    }
}
