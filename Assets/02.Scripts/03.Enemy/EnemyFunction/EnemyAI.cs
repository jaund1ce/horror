using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public enum SightInObject
{
    Player = 0,
    Object = 1
}

public enum AIState
{
    Idle,
    Wandering,
    Chasing,
    Attacking,
    Frenzy
}

public struct SoundState
{
    public AudioClip Sound;
    public float LastPlayTime; 
}

public abstract class EnemyAI : MonoBehaviour, IAggroGage
{
    [field: Header("Enemy Settings")]

    [SerializeField]protected LayerMask playerMask;
    [HideInInspector] public EnemySO Data { get; protected set; }
    [HideInInspector] public bool IsAttacking;
    protected Enemy enemy;
    protected NavMeshAgent agent;
    protected string doorTag = "Door";

    [field: Header("Enemy CalculateToAction")]

    [HideInInspector] public AIState EnemyAistate{ get; protected set; }
    [HideInInspector] public Dictionary<AIState, SoundState> soundStates = new Dictionary<AIState, SoundState>();
    [HideInInspector] public float AggroGage;
    [HideInInspector] public bool IsPlayerMiss { get; protected set; } = true;
    [HideInInspector] public bool IsAggroGageMax { get; protected set; }
    protected float checkMissTime;
    protected float checkAggroSoundTime;
    protected float releaseGageAmount;
    protected List<int> visionInObject = new List<int>();
    protected AIState previouseState;

    protected virtual void Awake()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        EnemyAistate = AIState.Idle;
        Data = enemy.Data;
        checkAggroSoundTime = 30f;
        releaseGageAmount = Data.MaxAggroGage/100;
    }

    protected virtual void Start() 
    {
        MainGameManager.Instance.MakeSoundAction += GetAggroGage;
        soundStates.Add(AIState.Idle, new SoundState { Sound = enemy.IdleSound, LastPlayTime = -enemy.SoundTime });
        soundStates.Add(AIState.Wandering, new SoundState { Sound = enemy.WanderSound, LastPlayTime = -enemy.SoundTime });
        soundStates.Add(AIState.Chasing, new SoundState { Sound = enemy.ChasingSound, LastPlayTime = -enemy.SoundTime });
        soundStates.Add(AIState.Attacking, new SoundState { Sound = enemy.AttackSound, LastPlayTime = -enemy.SoundTime });
        soundStates.Add(AIState.Frenzy, new SoundState { Sound = enemy.HowlingSound, LastPlayTime = -enemy.SoundTime });
    }

    protected virtual void Update()
    {
        CheckTarget();
        UpdateState();

        if (previouseState != EnemyAistate) 
        {
            PlaySoundBasedOnState();
            previouseState = EnemyAistate;
        }
;    }

    protected virtual void CheckTarget()
    {
        // 입력받은 각도에 따라 방향을 결정한다.
        bool isTarget = false;
        float halfAngle = Data.VisionRange / 2f;
        float rayToRay = Data.VisionRange / (Data.RayAmount - 1);

        for (int i = 0; i < Data.RayAmount; i++)
        {
            float currentAngle = -halfAngle + i * rayToRay;

            // 방향 벡터 계산 (로컬 좌표계 기준)
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            if (Physics.Raycast(transform.position - (Vector3.up * 0.2f), direction, out RaycastHit hit, Data.VisionDistance, playerMask))
            {
                if (MainGameManager.Instance.Player.isHiding) return;
                if (visionInObject.Count == 0)
                {
                    visionInObject.Add((int)SightInObject.Player);

                }
                isTarget = true;

                Debug.DrawLine(transform.position - (Vector3.up * 0.2f), hit.point, Color.red, 1.0f);
            }
            else
            {

               Debug.DrawRay(transform.position - (Vector3.up * 0.2f), direction * Data.VisionDistance, Color.green, 1.0f);
            }
        }

        CheckMissTime(isTarget);
        checkAggroSoundTime += Time.deltaTime;
    }

    protected virtual void CheckMissTime(bool isTarget)
    {
        if (!isTarget)
        {
            checkMissTime += Time.deltaTime;
            if (checkMissTime > Data.MissTargetTime)
            {
                if (!IsPlayerMiss) GetAggroGage(-0.3f * Data.MaxAggroGage);
                IsPlayerMiss = true;
            }
        }
        else
        {
            IsPlayerMiss = false;
            checkMissTime = 0;
        }
    }

    public virtual void FeelThePlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Data.FeelPlayerRange, playerMask);

        foreach (Collider collider in colliders)
        {
            float increaceAmount = 0;
            float distance = Vector3.Distance(collider.transform.position, transform.position);
            increaceAmount = Data.FeelPlayerRange / (distance * 100);
            GetAggroGage(increaceAmount);
        }

        if (colliders.Length == 0) RealeaseAggroGage(releaseGageAmount);
    }

    public virtual void GetAggroGage(float amount)
    {
        AggroGage += amount;
        AggroGage = Mathf.Clamp(AggroGage, 0f, 100f);


        if (AggroGage >= Data.MaxAggroGage)
        {
            IsAggroGageMax = true;
        }
        else
        {
            IsAggroGageMax = false;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(Data!=null) Gizmos.DrawWireSphere(transform.position, Data.FeelPlayerRange); 
    }

    protected virtual bool IsInAttackRange()
    {
        float playerDistanceSqr = (MainGameManager.Instance.Player.transform.position - transform.position).sqrMagnitude;
        return playerDistanceSqr <= Data.AttackRange * Data.AttackRange;
    }

    public virtual int UpdateState()
    {
        if (MainGameManager.Instance.Player.PlayerConditionController.IsDie)
        {
            EnemyAistate = AIState.Idle;
            return (int)EnemyAistate;
        }
        else if (MainGameManager.Instance.Player.isHiding)
        {
            EnemyAistate = AIState.Wandering;
            IsAttacking = false;
            return (int)EnemyAistate;
        }
        

        if (IsAttacking) return (int)EnemyAistate;

        if ((IsAggroGageMax || !IsPlayerMiss) && !IsInAttackRange())
        {
            EnemyAistate = AIState.Chasing;
            return (int)EnemyAistate;
        }
        else if (!IsAggroGageMax && IsPlayerMiss)
        {
            EnemyAistate = AIState.Wandering;
            FeelThePlayer();
            CheckHalfAggroGage();
            return (int)EnemyAistate;
        }
        else if (!IsPlayerMiss && IsInAttackRange())
        {
            EnemyAistate = AIState.Attacking;
            IsAttacking = true;
            return (int)EnemyAistate;
        }
        else
        {
            EnemyAistate = AIState.Idle;
            return (int)EnemyAistate;
        }
    }

    protected virtual void PlaySoundBasedOnState()
    {
        if (!soundStates.ContainsKey(EnemyAistate)) return;

        SoundState currentSoundState = soundStates[EnemyAistate];

        if (Time.time - currentSoundState.LastPlayTime >= enemy.SoundTime)
        {
            if (currentSoundState.Sound != null)
            {
                enemy.AudioSource.Stop();
                enemy.AudioSource.clip = currentSoundState.Sound;
                enemy.AudioSource.Play();

                currentSoundState.LastPlayTime = Time.time; // 구조체는 값 타입이므로 아래코드 업데이트 하기 위해 필요
                soundStates[EnemyAistate] = currentSoundState; 
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other) 
    {
        LockedDoor lockedDoor = other.GetComponent<LockedDoor>();

        if (lockedDoor != null)
        {
            if(!lockedDoor.IsOpened) lockedDoor.ToggleDoor();
        }
    }

    public virtual void RealeaseAggroGage(float amount)
    {
        AggroGage -= amount * Time.deltaTime;
        AggroGage = Mathf.Clamp(AggroGage, 0f, 100f);
    }

    protected virtual void CheckHalfAggroGage() 
    {
        if (AggroGage / Data.MaxAggroGage > 0.5f && checkAggroSoundTime > 30f) 
        {
            enemy.AudioSource.PlayOneShot(enemy.AggroSound);
            checkAggroSoundTime = 0f;
        }

    }
}

