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
    public InventoryData CurrentEquipItem;
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
        stateMachine.ChangeState(stateMachine.IdleState);//ó�� ���۽� idlestate�� ����
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

    void ChangeRotation()//���߿� �ٸ� �Ŵ����� �ű��?
    {
        if (Input.rotateSencitivity == Data.GroundData.BaseRotationDamping) return;
        else
        {
            Input.rotateSencitivity = Data.GroundData.BaseRotationDamping;
        }
    }

    private void CheckGround()
    {
        Ray ray1 = new Ray(this.gameObject.transform.position + Vector3.forward*0.1f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray2 = new Ray(this.gameObject.transform.position + Vector3.back * 0.1f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray3 = new Ray(this.gameObject.transform.position + Vector3.right * 0.1f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray4 = new Ray(this.gameObject.transform.position + Vector3.left * 0.1f + new Vector3(0,0.1f,0), Vector3.down);
        //Debug.DrawRay(this.gameObject.transform.position, Vector3.down, Color.red, 3f);

        if (Physics.Raycast(ray1, 0.2f) || Physics.Raycast(ray2, 0.2f) || Physics.Raycast(ray3, 0.2f) || Physics.Raycast(ray4, 0.2f))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}
