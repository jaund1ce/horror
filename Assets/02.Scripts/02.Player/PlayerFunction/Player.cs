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
    public Rigidbody PlayerRigidbody { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerConditionController health { get; private set; }

    private PlayerStateMachine2 stateMachine;

    public Action makeSound;
    public Action addItem;
    public Action useItem;
    public PlayerInventoryData playerInventoryData;
    public ItemData CurrentItemData;
    public bool isChangingQuickSlot = false;
    public bool isGround = true;
    public PlayerState PlayerState = PlayerState.Normal;

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Interact = GetComponentInChildren<PlayerInteraction>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        ForceReceiver = GetComponent<ForceReceiver>();
        health = GetComponent<PlayerConditionController>();
        playerInventoryData = GetComponent<PlayerInventoryData>();

        stateMachine = new PlayerStateMachine2(this);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);//처음 시작시 idlestate로 실행
        health.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
        ChangeRotation();//
        CheckGround();
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

    void ChangeRotation()//나중에 다른 매니져로 옮기기?
    {
        if (Input.rotateSencitivity == Data.GroundData.BaseRotationDamping) return;
        else
        {
            Input.rotateSencitivity = Data.GroundData.BaseRotationDamping;
        }
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))//모든 iteractable layer은 iinteractable을 가지고 있다.
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}
