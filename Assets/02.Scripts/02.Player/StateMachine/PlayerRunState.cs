using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Player.Input.isRunning = true;
        stateMachine.Player.Input.VirtualCameraNoise.m_AmplitudeGain = 5f;
        stateMachine.Player.Input.VirtualCameraNoise.m_FrequencyGain = 0.05f;
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        stateMachine.Player.Input.isRunning = false;
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!stateMachine.Player.Input.RunningReady)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
}
