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
        StartAnimation(stateMachine.Player.AnimationData.CrouchingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.CrouchingParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    //public override void PhysicsUpdate() //없어도 되나? crouchidle에 있으니까?
    //{
    //    base.PhysicsUpdate();
    //}
}
