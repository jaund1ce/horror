using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : MonoBehaviour
{
    [field: Header("Enemy Settings")]
    [field: SerializeField] public EnemySO Data { get; protected set; }
    [field: SerializeField] public AttackPoint AttackPoint { get; protected set; }
    public EnemyAI EnemyAI { get; protected set; }
    public NavMeshAgent CharacterController { get; protected set; }


    [field: Header("Enemy Sounds")]
    [field: SerializeField] public AudioSource AudioSource { get; protected set; }
    [field: SerializeField] public AudioClip IdleSound { get; protected set; }
    [field: SerializeField] public AudioClip WanderSound { get; protected set; }
    [field: SerializeField] public AudioClip ChasingSound { get; protected set; }
    [field: SerializeField] public AudioClip HowlingSound { get; protected set; }
    [field: SerializeField] public AudioClip AttackSound { get; protected set; }
    [HideInInspector]public float SoundTime { get; protected set; } = 5f;

    protected EnemyStateMachine stateMachine;


    [field: Header("Enemy Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData;
    public Animator EnemyAnimator { get; protected set; }

    protected virtual void Awake() 
    {
        AnimationData.Initialize();
        EnemyAnimator = GetComponent<Animator>();
        CharacterController = GetComponent<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
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

