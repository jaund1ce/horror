using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : MonoBehaviour
{
    [field: SerializeField] public CreatureSO Data { get; protected set; }

    [field: SerializeField] public AttackPoint AttackPoint { get; protected set; }
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData;

    public Animator CreatureAnimator { get; protected set; }

    public NavMeshAgent CharacterController { get; set; }

    public ForceReceiver ForceReceiver { get; protected set; }
    protected EnemyStateMachine stateMachine;
    public EnemyAI EnemyAI { get; protected set; }

    protected virtual void Awake() 
    {
        AnimationData.Initialize();
        CreatureAnimator = GetComponent<Animator>();
        CharacterController = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();
    }

    protected virtual void Start() 
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected virtual void Update() 
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    protected virtual void FixedUpdate() 
    {
        stateMachine.PhysicsUpdate();
    }
}

