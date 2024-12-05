using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
    public enum SightInObject 
    {
        Player = 0,
        Object = 1
    }

    public enum AIState 
    {
        Idle,
        Wandering,
        Chasing
    }


public class CreatureAI : MonoBehaviour , AggroGage
{
    

    public LayerMask player;
    private NavMeshAgent agent;
    public AIState CreatureAistate;
    [field: SerializeField] public CreatureSO Data { get; private set; }


    public bool isPlayerMiss { get; private set; }
    public bool IsAggroGageMax { get; private set; }
    private float checkMissTime;
    private float aggroGage;
    
    private List<int> visionInObject = new List<int>(); 


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //visionInObject = new List<int>();
    }

    private void Update()
    {
        CheckTarget();
        UpdateState();

    }

    private void CheckTarget() 
    {
        // 입력받은 각도에 따라 방향을 결정한다.
        bool isTarget = false;
        float halfAngle = Data.VisionRange / 2f;
        float rayToRay = Data.VisionRange / (Data.RayAmount - 1);

        for (int i = 0; i < Data.RayAmount; i++) 
        {
            // 현재 레이의 각도
            float currentAngle = - halfAngle + i * rayToRay;

            // 방향 벡터 계산 (로컬 좌표계 기준)
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            // 레이 발사
            if (Physics.Raycast(transform.position+(Vector3.up*0.5f), direction, out RaycastHit hit, Data.VisionDistance, player))
            {
                Debug.Log($"Hit: {hit.collider.name}");


                 if (visionInObject.Count == 0)
                 {
                     visionInObject.Add((int)SightInObject.Player); 

                 }
                isTarget = true;

                Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point, Color.red, 1.0f);
            }
            else
            {

                Debug.DrawRay(transform.position + (Vector3.up * 0.5f), direction * Data.VisionDistance, Color.green, 1.0f);
            }
        }

        CheckMissTime(isTarget);

    }

    private void CheckMissTime(bool isTarget)
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
            checkMissTime = 0;
        }
    }

    public void GetAggroGage(float amount)
    {

        aggroGage += amount;

        if (aggroGage >= Data.MaxAggroGage) 
        {
            IsAggroGageMax = true;
        }
        else 
        {
            IsAggroGageMax = false;
        }
    }

    public void FeelThePlayer() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Data.FeelPlayerRange , player);

        foreach (Collider collider in colliders) 
        {
            float increaceAmount = 0;
            Vector3 direction = (collider.transform.position - transform.position).normalized;
            increaceAmount = (MathF.Abs(direction.x) + MathF.Abs(direction.z))/Data.FeelPlayerRange;
            GetAggroGage(increaceAmount);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Data.FeelPlayerRange);
    }

    public int UpdateState() 
    {
        if (IsAggroGageMax || !isPlayerMiss)
        {
            CreatureAistate = AIState.Chasing;
        } 
        else if (!IsAggroGageMax && isPlayerMiss) 
        {
            CreatureAistate = AIState.Wandering;
            FeelThePlayer();
        }

        return (int)CreatureAistate;
    }
    
}

