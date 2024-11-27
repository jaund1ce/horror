using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatureBaseState : MonoBehaviour // : IState
{
    protected CreatureStateMachine stateMachine;
    // protected readonly PlayerGroundData groundData;

    public CreatureBaseState(CreatureStateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
        //groundData = stateMachine.Player.Data.GroundData;
    }

    /*public void Enter() { }
    public void Exit() { }
    public void HandleInput() { }
    public void PhysicsUpdate() { }*/
    public void Update() { }
    public void StartAnimation(int animatorHash) 
    {
        stateMachine.Creature.CreatureAnimator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash) 
    {
        stateMachine.Creature.CreatureAnimator.SetBool(animatorHash, false);
    }

    /*private void Move() 
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;

    }*/

    private void Move(Vector3 direction) 
    {
    
    }

    private void Rotate(Vector3 direction)
    {

    }

}
