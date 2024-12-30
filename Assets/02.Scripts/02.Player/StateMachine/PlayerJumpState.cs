using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine2 stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.PlayerRigidbody.AddForce(Vector3.up * stateMachine.JumpForce, ForceMode.Impulse);//jumpforce 라는 숫자로 바꿔야 함
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    //public override void Update() //공중에서의 이동을 순간적으로만 줄것이기 때문에 불필요.
    //{
    //    base.Update();
    //}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    } 
}
