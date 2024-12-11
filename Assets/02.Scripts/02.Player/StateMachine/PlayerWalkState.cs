using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.Player.Input.IsRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        else
        {
            stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
        }        
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        stateMachine.ChangeState(stateMachine.RunState);
    }
}
