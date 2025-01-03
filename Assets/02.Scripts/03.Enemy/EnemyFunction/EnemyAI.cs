using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

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

public abstract class EnemyAI : MonoBehaviour, IAggroGage
{
    
    [SerializeField]protected LayerMask playerMask;
    [HideInInspector] public AIState EnemyAistate;
    protected NavMeshAgent agent;
    [field: SerializeField] public EnemySO Data { get; private set; }

    public bool isPlayerMiss { get; private set; } = true;
    public bool IsAggroGageMax { get; private set; }
    protected float checkMissTime;
    [HideInInspector] public float AggroGage;
    [HideInInspector] public bool IsAttacking;

    protected List<int> visionInObject = new List<int>();

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyAistate = AIState.Idle;
    }

    protected virtual void Start() 
    {
        MainGameManager.Instance.MakeSoundAction += GetAggroGage;
    }

    protected virtual void Update()
    {
        CheckTarget();
        UpdateState();

    }

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
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), direction, out RaycastHit hit, Data.VisionDistance, playerMask))
            {
                if (visionInObject.Count == 0)
                {
                    visionInObject.Add((int)SightInObject.Player);

                }
                isTarget = true;

                Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point, Color.red, 1.0f);
            }
            else
            {

               //Debug.DrawRay(transform.position + (Vector3.up * 0.5f), direction * Data.VisionDistance, Color.green, 1.0f);
            }
        }

        CheckMissTime(isTarget);

    }

    protected virtual void CheckMissTime(bool isTarget)
    {
        if (!isTarget)
        {
            checkMissTime += Time.deltaTime;
            if (checkMissTime > Data.MissTargetTime)
            {
                isPlayerMiss = true;
            }
        }
        else
        {
            isPlayerMiss = false;
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
    }

    public virtual void GetAggroGage(float amount)
    {
        AggroGage += amount;

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
        Gizmos.DrawWireSphere(transform.position, Data.FeelPlayerRange);
    }

    protected virtual bool IsInAttackRange()
    {
        float playerDistanceSqr = (MainGameManager.Instance.Player.transform.position - transform.position).sqrMagnitude;
        return playerDistanceSqr <= Data.AttackRange * Data.AttackRange;
    }

    public virtual int UpdateState()
    {
        if (IsAttacking) return (int)EnemyAistate;

        if ((IsAggroGageMax || !isPlayerMiss) && !IsInAttackRange())
        {
            EnemyAistate = AIState.Chasing;
            return (int)EnemyAistate;
        }
        else if (!IsAggroGageMax && isPlayerMiss)
        {
            EnemyAistate = AIState.Wandering;
            FeelThePlayer();
            return (int)EnemyAistate;
        }
        else if (!isPlayerMiss && IsInAttackRange())
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
}

