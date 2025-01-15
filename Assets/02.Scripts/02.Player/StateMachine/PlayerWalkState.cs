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
        if (stateMachine.Player.Input.RunningReady)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        else
        {
            base.Enter();
            ChangePlayerStateEnter();
            StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
        }        
    }

    public override void Exit()
    {
        SoundManger.Instance.PlayPlayerStepSound(false);
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        stateMachine.ChangeState(stateMachine.RunState);
    }
    private void ChangePlayerStateEnter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        stateMachine.Player.Input.VirtualCameraNoise.m_AmplitudeGain = 2f;
        stateMachine.Player.Input.VirtualCameraNoise.m_FrequencyGain = 0.02f;
        SoundManger.Instance.PlayPlayerStepSound(true, 0.5f);
    }

}
