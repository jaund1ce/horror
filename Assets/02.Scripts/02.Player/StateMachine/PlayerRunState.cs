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
        base.Enter();
        ChangePlayerStateEnter();
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        ChangePlayerStateExit();
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

    private void ChangePlayerStateEnter()
    {
        SoundManger.Instance.PlayPlayerStepSound(true, 1f);
        stateMachine.isRunning = true;
        stateMachine.Player.Input.VirtualCameraNoise.m_AmplitudeGain = 5f;
        stateMachine.Player.Input.VirtualCameraNoise.m_FrequencyGain = 0.05f;
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
    }

    private void ChangePlayerStateExit()
    {
        stateMachine.isRunning = false;
        SoundManger.Instance.PlayPlayerStepSound(false);
    }
}
