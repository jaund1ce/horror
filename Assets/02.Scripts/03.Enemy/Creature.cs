using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Creature : MonoBehaviour
{
    [field: SerializeField] public CreatureSO Data { get; private set; }
    public CreatureAI CreatureAI { get; private set; }
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData;

    public Animator CreatureAnimator { get; private set; }
    public NavMeshAgent CharacterController { get; set; }

    public ForceReceiver ForceReceiver { get; private set; }
    private CreatureStateMachine stateMachine;

    // Start is called before the first frame update
    void Awake()
    {
        AnimationData.Initialize();
        CreatureAnimator = GetComponent<Animator>();
        CharacterController = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();
        CreatureAI = GetComponent<CreatureAI>();

        stateMachine = new CreatureStateMachine(this);

    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}
