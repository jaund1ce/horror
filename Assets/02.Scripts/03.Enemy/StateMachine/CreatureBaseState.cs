using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CreatureBaseState : IState
{
    protected CreatureStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    protected float MovementSpeedModifier = 1f;
    private Vector3 movementLocation = Vector3.zero;
    private Transform creatureTransform;
    private float minWanderDistance;
    private float maxWanderDistance;
    private bool setLocation;
    int walkableMask;

    public CreatureBaseState(CreatureStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Creature.Data.GroundData;
        creatureTransform = stateMachine.Creature.gameObject.transform;
        minWanderDistance = stateMachine.Creature.Data.MinWanderDistance;
        maxWanderDistance = stateMachine.Creature.Data.MaxWanderDistance;
        walkableMask = NavMesh.GetAreaFromName("walkable");
        
    }

    public virtual void Enter() 
    {
        stateMachine.Creature.CharacterController.speed = stateMachine.Creature.Data.GroundData.BaseSpeed * MovementSpeedModifier;
    }
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
            if (!IsLocationSet()) 
            {
                WanderLocationSet();
            }
            Move();
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
        if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Chasing)
        {
            movementLocation = stateMachine.Target.transform.position;
        }
        Move(movementLocation);
        //Rotate(movementLocation);
    }

    private void WanderLocationSet()
    {
        NavMeshHit hit;
        Vector3 radius = Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
        radius.y = 0;
        //sourcePosition : 일정한 영역 hit : 이동할수있는 경로의 최단 경로 
        Vector3 randomPosition = creatureTransform.position + radius;

        if (NavMesh.SamplePosition(randomPosition, out hit, maxWanderDistance, walkableMask) == false) return;
            movementLocation = hit.position;
            Debug.Log($"New Position : {movementLocation}");
            setLocation = true;
    }

    private bool IsLocationSet() 
    {
        if (Vector3.Distance(creatureTransform.position, movementLocation) < 1f || movementLocation == Vector3.zero)
        {
            setLocation = false;
        }

        return setLocation;
    }

    /*private Vector3 GetMovementDirection()
    {
        Vector3 dir = stateMachine.Target.transform.position;

        return dir;

    }*/

    private void Move(Vector3 direction)
    {
        //stateMachine.Creature.CharacterController.SetDestination(((direction * movementSpeed) + stateMachine.Creature.ForceReceiver.Movement) * Time.deltaTime);
        stateMachine.Creature.CharacterController.SetDestination(direction);
        Debug.Log(stateMachine.Creature.CharacterController.speed);

    }


/*    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform creatureTransform = stateMachine.Creature.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            creatureTransform.rotation = Quaternion.Slerp(creatureTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }*/

    protected void ForceMove()
    {
        stateMachine.Creature.CharacterController.SetDestination(stateMachine.Creature.ForceReceiver.Movement * Time.deltaTime);
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
