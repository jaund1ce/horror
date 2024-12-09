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
            //�и� ����
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
        //������Ʈ���� �����߿� ���� �־����� chasing���� �Ѿ�� �Ұ��� �־� üũ��
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
        //sourcePosition : ������ ���� hit : �̵��Ҽ��ִ� ����� �ִ� ��� 
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
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 �� BaseLayer �߰��Ǹ� 1...
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        //��ȯ �ǰ� ������ && ���� �ִϸ��̼��� tag
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        //��ȯ �ǰ� ���� ���� �� && ���� �ִϸ��̼��� tag
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
