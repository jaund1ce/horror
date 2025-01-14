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
        stateMachine.isRunning = true;
        stateMachine.Player.Input.VirtualCameraNoise.m_AmplitudeGain = 5f;
        stateMachine.Player.Input.VirtualCameraNoise.m_FrequencyGain = 0.05f;
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        SoundManger.Instance.PlayPlayrtStepSound(true, 1f);
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        stateMachine.isRunning = false;
        SoundManger.Instance.PlayPlayrtStepSound(false);
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
