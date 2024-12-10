using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public PlayerSO Data {  get; private set; }

    [field:Header("Animations")]
    [field:SerializeField] public PlayerAnimationData AnimationData {  get; private set; }

    public Animator Animator { get; private set; }
    public PlayerController Input { get; private set; }
    public PlayerInteraction Interact { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Health health { get; private set; }

    private PlayerStateMachine2 stateMachine;

    public Action makeSound;
    public Action addItem;
    public Action useItem;
    public PlayerInventoryData playerInventoryData;
    public bool isChangingQuickSlot = false;

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Interact = GetComponentInChildren<PlayerInteraction>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        health = GetComponent<Health>();
        playerInventoryData = GetComponent<PlayerInventoryData>();

        stateMachine = new PlayerStateMachine2(this);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
        health.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
        ChangeRotation();//
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

    void ChangeRotation()//���߿� �ٸ� �Ŵ����� �ű��?
    {
        if (Input.rotateXSencitivity == Data.GroundData.BaseRotationDamping * 6 && Input.rotateYSencitivity == Data.GroundData.BaseRotationDamping) return;
        else
        {
            Input.rotateXSencitivity = Data.GroundData.BaseRotationDamping * 6;
            Input.rotateYSencitivity = Data.GroundData.BaseRotationDamping;
        }
    }
}
