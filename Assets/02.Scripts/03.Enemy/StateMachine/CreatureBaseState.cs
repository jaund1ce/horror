using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
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
        
    }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Update()
    {
        stateMachine.Creature.CharacterController.speed = stateMachine.Creature.Data.GroundData.BaseSpeed * MovementSpeedModifier;

        switch (stateMachine.Creature.CreatureAI.CreatureAistate) 
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
                    WanderLocationSet();
                }
                stateMachine.ChangeState(stateMachine.WanderState);
                Move();

                break;
            case AIState.Attacking:

                stateMachine.ChangeState(stateMachine.AttackState);

                break;

        }
            
        /*if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Idle )
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Chasing )
        {
            //분리 예정
            stateMachine.ChangeState(stateMachine.ChasingState);
            Move();
        }
        else if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Wandering )
        {
            if (!IsLocationSet())
            {
                WanderLocationSet();
            }
            stateMachine.ChangeState(stateMachine.WanderState);
            Move();
        }
        //업데이트에서 공격중에 적이 멀어지면 chasing으로 넘어가서 불값도 넣어 체크중
        else if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Attacking)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }*/

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
            setLocation = true;
    }

    private bool IsLocationSet() 
    {
        if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Chasing) 
        {
            setLocation = true;
        }
        if (Vector3.Distance(creatureTransform.position, movementLocation) < 1f || movementLocation == Vector3.zero )
        {
            setLocation = false;
        }

        return setLocation;
    }


    private void Move(Vector3 direction)
    {      
        stateMachine.Creature.CharacterController.SetDestination(direction);
        //Debug.Log(stateMachine.Creature.CharacterController.speed);

    }


    /*protected void ForceMove()
    {
        stateMachine.Creature.CharacterController.SetDestination(stateMachine.Creature.ForceReceiver.Movement * Time.deltaTime);
    }*/

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

}
