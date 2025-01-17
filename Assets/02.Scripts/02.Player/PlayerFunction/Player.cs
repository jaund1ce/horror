using System;
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
    public PlayerConditionController PlayerConditionController { get; private set; }

    [HideInInspector]public PlayerStateMachine2 StateMachine;
    public PlayerInventoryData PlayerInventoryData;
    public InventoryData CurrentEquipItem;

    public event Action HPChange;

    [Header("Player States")]
    public bool isChangingQuickSlot = false;
    public bool isInventoryOpen = false;
    public bool isGround = true;
    public bool isHiding = false;
    public bool isCollideOverlapr= false;
    [SerializeField]private PlayerHeartState playerState = PlayerHeartState.Normal; //creture 와 플레이어가 둘다 가지고 있어야하나?
    [SerializeField]private GroundType groundType = GroundType.Cement;
    [SerializeField] private float groundCheckdistance = 0.4f;

    [Header("Monster Check Data")]
    [SerializeField] private float checkDistance = 30f;
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
        PlayerConditionController = GetComponent<PlayerConditionController>();
        PlayerInventoryData = GetComponent<PlayerInventoryData>();

        StateMachine = new PlayerStateMachine2(this);
        MainGameManager.Instance.Player=this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StateMachine.ChangeState(StateMachine.IdleState);//처음 시작시 idlestate로 실행
        PlayerConditionController.OnDie += OnDie;
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
        CheckGround();

        if (Time.time - lastCheckTime < checkDuration) return;
        CheckMonster();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            if (isHiding) isCollideOverlapr = true;
            isHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            if (isCollideOverlapr)
            {
                isCollideOverlapr = false;
                isHiding = true;
                return;
            }
            isHiding = false;
        }
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

    private void CheckGround()
    {
        Vector3 curVector = this.gameObject.transform.position;
        Ray ray1 = new Ray(curVector + Vector3.forward*0.2f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray2 = new Ray(curVector + Vector3.back * 0.2f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray3 = new Ray(curVector + Vector3.right * 0.2f + new Vector3(0,0.1f,0), Vector3.down);
        Ray ray4 = new Ray(curVector + Vector3.left * 0.2f + new Vector3(0,0.1f,0), Vector3.down);
        
        RaycastHit hit;

        if (Physics.Raycast(ray1,out hit, groundCheckdistance) || Physics.Raycast(ray2, groundCheckdistance) || Physics.Raycast(ray3, groundCheckdistance) || Physics.Raycast(ray4, groundCheckdistance))
        {
            if (hit.collider != null)
            {
                GroundType temGrounType = GroundType.None;

                switch (hit.collider.gameObject.layer)
                {
                    case 0:
                    case 20: temGrounType = GroundType.Cement; break;
                    case 21: temGrounType = GroundType.Concrete; break;
                    case 22: temGrounType = GroundType.Wood; break;
                    case 23: temGrounType = GroundType.Dirt; break;
                    default:
                        {
                            Debug.Log("No GroundType!");
                            groundType = GroundType.None;
                            break;
                        }
                }

                if (temGrounType != groundType)
                {
                    groundType = temGrounType;
                    SoundManger.Instance.ChangeStepSound(groundType);
                }
            }
            isGround = true;            
        }
        else
        {
            groundType = GroundType.None;
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
        if (this.playerState == playerState) return;

        this.playerState = playerState;  
        SoundManger.Instance.ChangeHearthBeatSound(this.playerState);
    }

    public bool CheckState(PlayerHeartState playerState)
    {
        return this.playerState == playerState ? true : false;
    }

    public void ChangeEquip()
    {
        Animator.SetBool("FlashLight", false);
        Animator.SetBool("HealPack", false);
        Animator.SetBool("Key", false);

        if (CurrentEquipItem == null || CurrentEquipItem.ItemData == null) return;

        if (CurrentEquipItem.ItemData.itemSO.ItemNameEng == "flash")
        {
            Animator.SetBool("FlashLight", true);
        }
        else if (CurrentEquipItem.ItemData.itemSO.ItemNameEng == "healpack")
        {
            Animator.SetBool("HealPack", true);
        }
        else
        {
            Animator.SetBool("Key", true);
        }
    }

    public void EquipItem(InventoryData inventoryData)
    {
        if(Input.EquipMent.CurEquip.OnUsing)
        {
            return;
        }
        CurrentEquipItem = inventoryData;
        Input.EquipMent.EquipNew(CurrentEquipItem);
        ChangeEquip();
    }

    public void UnEquipCurrentItem()
    {
        if (Input.EquipMent.CurEquip.OnUsing)
        {
            return;
        }
        Input.EquipMent.UnEquip();
        CurrentEquipItem = null;
        ChangeEquip();
    }

    public void OnHPChange()
    {
        HPChange?.Invoke();
    }
}
