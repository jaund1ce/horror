
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        stateMachine.Player.PlayerRigidbody.AddForce(Vector3.down * 100f * Time.deltaTime, ForceMode.Impulse); //왜인지는 모르나 accelate를 써도 적용이 안되어서 impurse로 비슷한 느낌을 냄
        Debug.Log(stateMachine.Player.PlayerRigidbody.velocity.y);

        if (stateMachine.Player.isGround)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
