using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("idlestate");
        ChangePlayerStateEnter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (stateMachine.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()//외부 요인으로 상태가 변할 수 있기 떄문에
    {
        base.Update();
    }

    private void ChangePlayerStateEnter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        stateMachine.Player.Input.VirtualCameraNoise.m_AmplitudeGain = 1f;
        stateMachine.Player.Input.VirtualCameraNoise.m_FrequencyGain = 0.01f;
    }
}
