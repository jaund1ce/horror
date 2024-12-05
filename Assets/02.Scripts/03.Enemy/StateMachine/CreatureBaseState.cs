using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CreatureBaseState  : IState
{
    protected CreatureStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    private Vector3 movementLocation = Vector3.zero;
    private Vector3 creatureTransform;
    private float minWanderDistance;
    private float maxWanderDistance;
    int walkableMask;
    public CreatureBaseState(CreatureStateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Creature.Data.GroundData;
        creatureTransform = stateMachine.Creature.transform.position;
        minWanderDistance = stateMachine.Creature.Data.MinWanderDistance;
        maxWanderDistance = stateMachine.Creature.Data.MaxWanderDistance;
        walkableMask = NavMesh.GetAreaFromName("walkable");
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Update() 
    {
        if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Idle)
        {

        }
        else if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Chasing)
        {
            Move();
        } 
        else if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Wandering) 
        {
            WanderLocationMove();
        }
        
    }


    public void StartAnimation(int animatorHash) 
    {
        stateMachine.Creature.CreatureAnimator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash) 
    {
        stateMachine.Creature.CreatureAnimator.SetBool(animatorHash, false);
    }

    private void Move() 
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
        Rotate(movementDirection);
    }
    private void WanderLocationMove()
    {
        NavMeshHit hit;

        //sourcePosition : 일정한 영역 hit : 이동할수있는 경로의 최단 경로 
        Vector3 randomPosition = creatureTransform + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance));
        NavMesh.SamplePosition(randomPosition, out hit, maxWanderDistance, walkableMask);

        if (Vector3.Distance(hit.position, creatureTransform) < 0.1f || movementLocation == Vector3.zero)
        {
            movementLocation = new Vector3(hit.position.x,0,hit.position.z);
            Debug.Log($"New Position : {movementLocation}");
        }

        Move(movementLocation);
        Rotate(movementLocation);
        

    }

    private Vector3 GetMovementDirection()
    {
        Vector3 dir = (stateMachine.Target.transform.position - stateMachine.Creature.transform.position);

        return dir;

    }

    private void Move(Vector3 direction) 
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Creature.CharacterController.Move(((direction * movementSpeed) + stateMachine.Creature.ForceReceiver.Movement) * Time.deltaTime);
    }

    private float GetMovementSpeed() 
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform creatureTransform = stateMachine.Creature.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            creatureTransform.rotation = Quaternion.Slerp(creatureTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected void ForceMove() 
    {
        stateMachine.Creature.CharacterController.Move(stateMachine.Creature.ForceReceiver.Movement * Time.deltaTime);
    }

    protected float GetNormalizedTime(Animator animator, string tag) 
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else 
        {
            return 0f;
        }
    }

    protected bool IsInChasingRange() 
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Creature.transform.position).sqrMagnitude; 
        // 나와 플레이어의 벡터의 크기 Magnitude는 제곱근 sqrMagnitude는 제곱근 하지 않은 연산. 고로 연산 자체가 덜 된다
        return playerDistanceSqr <= stateMachine.Creature.Data.PlayerChasingRange * stateMachine.Creature.Data.PlayerChasingRange;
    }
}
