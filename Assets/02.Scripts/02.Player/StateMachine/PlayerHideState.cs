using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideState : PlayerBaseState
{
    public PlayerHideState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.HideParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.HideParameterHash);
    }
}
