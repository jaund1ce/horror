using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected float MovementSpeedModifier = 1f;

    private Vector3 movementLocation = Vector3.zero;
    private Transform enemyTransform;
    private Transform target;
    private float minWanderDistance;
    private float maxWanderDistance;
    private bool setLocation;
    private int walkableMask;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Enemy.Data.GroundData;
        enemyTransform = stateMachine.Enemy.gameObject.transform;
        target = stateMachine.Target.transform;
        minWanderDistance = stateMachine.Enemy.Data.MinWanderDistance;
        maxWanderDistance = stateMachine.Enemy.Data.MaxWanderDistance;

        if (stateMachine.Enemy.Data.EnemyType == EnemyType.Ghost)
        {
            walkableMask = NavMesh.AllAreas;
        }
        else 
        {
            walkableMask = NavMesh.GetAreaFromName("walkable");
        }
        
    }

    public virtual void Enter() 
    {
        
    }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Update()
    {
        stateMachine.Enemy.CharacterController.speed = stateMachine.Enemy.Data.GroundData.BaseSpeed * MovementSpeedModifier;

        switch (stateMachine.Enemy.EnemyAI.EnemyAistate) 
        {
            case AIState.Idle:

                stateMachine.ChangeState(stateMachine.IdleState);

                break;

            case AIState.Chasing:

                stateMachine.ChangeState(stateMachine.ChasingState);

                Move();
                break;

            case AIState.Wandering:
                
                if (!IsLocationSet())
                {
                    WanderLocationSet(enemyTransform.position);
                }
                stateMachine.ChangeState(stateMachine.WanderState);
                Move();

                break;

            case AIState.Attacking:

                stateMachine.ChangeState(stateMachine.AttackState);

                break;

            case AIState.Frenzy:
                NearPlayerLocationSet();
                stateMachine.ChangeState(stateMachine.FrenzyState);
                break;
        }

    }


    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Enemy.EnemyAnimator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Enemy.EnemyAnimator.SetBool(animatorHash, false);
    }

    private void Move()
    {
        if (stateMachine.Enemy.EnemyAI.EnemyAistate == AIState.Chasing)
        {
            movementLocation = target.position;
        }
        Move(movementLocation);
    }

    private void NearPlayerLocationSet() 
    {
        if (stateMachine.Enemy.EnemyAI.EnemyAistate == AIState.Frenzy)
        {
            WanderLocationSet(GetRandomPointBetween());
        }
        Move(movementLocation);
    }

    private Vector3 GetRandomPointBetween() 
    {
        Vector3 direction = (target.position - enemyTransform.position).normalized;
        Vector3 point = enemyTransform.position + direction * 30f;
        WanderLocationSet(point);
        return point;
    }

    private void WanderLocationSet(Vector3 direction)
    {
        NavMeshHit hit;
        Vector3 radius = Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
        radius.y = 0f;
        Vector3 randomPosition = direction + radius;

        if (NavMesh.SamplePosition(randomPosition, out hit, maxWanderDistance, walkableMask) == false) return;
            movementLocation = hit.position;
            setLocation = true;
    }

    private bool IsLocationSet() 
    {
        if (stateMachine.Enemy.EnemyAI.EnemyAistate == AIState.Chasing) 
        {
            setLocation = true;
        }
        if (Vector3.Distance(enemyTransform.position, movementLocation) < 2f || movementLocation == Vector3.zero )
        {
            setLocation = false;
        }

        return setLocation;
    }


    private void Move(Vector3 direction)
    {
        stateMachine.Enemy.CharacterController.SetDestination(direction);
    }
 


    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 은 BaseLayer 추가되면 1...
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        //전환 되고 있을때 && 다음 애니메이션이 tag
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        //전환 되고 있지 않을 때 && 현재 애니메이션이 tag
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime%1f;
        }
        else
        {
            return 0f;
        }
    }

    protected void LookRotate()
    {
        Vector3 direction = target.position - enemyTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float rotationDamping = stateMachine.Enemy.Data.GroundData.BaseRotationDamping * Time.deltaTime;
        enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, lookRotation, rotationDamping);
    }

}
