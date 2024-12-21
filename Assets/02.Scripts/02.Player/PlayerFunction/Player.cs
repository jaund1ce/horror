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
    public CapsuleCollider CapsuleCollider { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerConditionController playerConditionController { get; private set; }

    private PlayerStateMachine2 stateMachine;

    public Action<float> makeSound;
    public PlayerInventoryData playerInventoryData;
    public InventoryData CurrentEquipItem;
    public bool isChangingQuickSlot = false;
    public bool isGround = true;
    [SerializeField]private PlayerState PlayerState = PlayerState.Normal; //creture 와 플레이어가 둘다 가지고 있어야하나?

    void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Interact = GetComponentInChildren<PlayerInteraction>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        ForceReceiver = GetComponent<ForceReceiver>();
        playerConditionController = GetComponent<PlayerConditionController>();
        playerInventoryData = GetComponent<PlayerInventoryData>();

        stateMachine = new PlayerStateMachine2(this);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);//처음 시작시 idlestate로 실행
        playerConditionController.OnDie += OnDie;
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
        CheckGround();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            ChangeState(PlayerState.Hide);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            ChangeState(PlayerState.Normal); 
        }
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

    void ChangeRotation()//나중에 다른 매니져로 옮기기? 환경설정에 넣기
    {
        if (Input.rotateSencitivity == Data.GroundData.BaseRotationDamping) return;
        else
        {
            Input.rotateSencitivity = Data.GroundData.BaseRotationDamping;
        }
    }

    private void CheckGround()
    {
        Vector3 curVector = this.gameObject.transform.position;
        Ray ray1 = new Ray(curVector + Vector3.forward*0.1f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray2 = new Ray(curVector + Vector3.back * 0.1f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray3 = new Ray(curVector + Vector3.right * 0.1f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray4 = new Ray(curVector + Vector3.left * 0.1f + new Vector3(0,0.1f,0), Vector3.down);
        float checkdistance = 0.3f;
        Debug.DrawRay(curVector, Vector3.down, Color.red, checkdistance);

        if (Physics.Raycast(ray1, checkdistance) || Physics.Raycast(ray2, checkdistance) || Physics.Raycast(ray3, checkdistance) || Physics.Raycast(ray4, checkdistance))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    public void MakeSound(float amount)
    {
        makeSound?.Invoke(amount);
    }

    public void ChangeState(PlayerState playerState)//아래 단계로는 변화 할 수 없지만, 기본 상태로 돌리는 건 가능하다.
    {
        if (playerState == PlayerState.Normal)
        {
            PlayerState = playerState;
        }
        else if (PlayerState > playerState) return;

        PlayerState = playerState;  
        SoundManger.Instance.ChangeState(playerState);
    }

    public bool CheckState(PlayerState playerState)
    {
        return PlayerState == playerState ? true : false; //한줄로 만들기 멋지잖아 한잔해 - merge 용
    }
}
