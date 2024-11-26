using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBaseState : MonoBehaviour // : IState
{
    protected CreatureStateMachine stateMachine;
    // protected readonly PlayerGroundData groundData;

    public CreatureBaseState(CreatureStateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
        //groundData = stateMachine.Player.Data.GroundData;
    }

    public void Enter() { }
    public void Exit() { }
    public void HandleInput() { }
    public void PhysicsUpdate() { }
    public void Update() { }

}
