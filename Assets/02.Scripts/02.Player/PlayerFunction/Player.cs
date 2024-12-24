using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
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
    public PlayerInventoryData playerInventoryData;
    public InventoryData CurrentEquipItem;

    [Header("Player States")]
    public bool isChangingQuickSlot = false;
    public bool isGround = true;
    public bool isHiding = false;
    [SerializeField]private PlayerHeartState playerState = PlayerHeartState.Normal; //creture 와 플레이어가 둘다 가지고 있어야하나?

    [Header("Monster Check Data")]
    [SerializeField] private float checkDistance = 12f;
    [SerializeField] private float checkDuration = 2f;
    [SerializeField] private LayerMask monsterMask;
    private float lastCheckTime = 0f;

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
        ChangeRotationSencitivity();//     ***   
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
        CheckGround();

        if (Time.time - lastCheckTime < checkDuration) return;
        CheckMonster();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            isHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            isHiding = false;
        }
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

    void ChangeRotationSencitivity()//나중에 다른 매니져로 옮기기? 환경설정에 넣기****************
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

    private void CheckMonster()
    {
        Vector3 curVector = this.gameObject.transform.position;

        Collider[] colliders = Physics.OverlapSphere(curVector, checkDistance, monsterMask);

        if (colliders.Length == 0)
        {
            ChangeState(PlayerHeartState.Normal);
        }
        else
        {
            float minDistance = checkDistance;
            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(curVector, collider.transform.position);
                minDistance = minDistance < distance ? minDistance : distance;
            }

            if (minDistance < checkDistance / 3)
            {
                ChangeState(PlayerHeartState.Chasing);
            }
            else if (minDistance < (checkDistance / 3) * 2)
            {
                ChangeState(PlayerHeartState.Danger);
            }
            else if (minDistance < checkDistance)
            {
                ChangeState(PlayerHeartState.Near);
            }
        }
    }

    public void ChangeState(PlayerHeartState playerState)
    {
        //if (playerState == PlayerState.Normal)//아래 단계로는 변화 할 수 없지만, 기본 상태로 돌리는 건 가능하다.
        //{
        //    this.playerState = playerState;
        //}
        //else if (this.playerState > playerState) return;
        if (this.playerState == playerState) return;

        this.playerState = playerState;  
        SoundManger.Instance.ChangeHearthBeatSound(playerState);
    }

    public bool CheckState(PlayerHeartState playerState)
    {
        return this.playerState == playerState ? true : false;
    }
}
