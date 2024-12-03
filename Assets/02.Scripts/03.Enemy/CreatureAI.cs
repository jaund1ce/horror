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
using static UnityEngine.UI.Image;


public class CreatureAI : MonoBehaviour , AggroGage
{
    public enum SightInObject 
    {
        Player = 0,
        Object = 1
    }

    public LayerMask player;
    private NavMeshAgent agent;

    [field: Header("CreatureSight")]
    [field: SerializeField] private float visionRange = 120f;
    [field: SerializeField] private float visionDistance = 10f;
    [field: SerializeField] private int rayAmount = 30;
    [field: SerializeField] private float missTargetTime = 3f;


    public bool isPlayerMiss { get; private set; }
    private float checkMissTime;
    private float aggroGage;
    private List<int> visionInObject = new List<int>();

    public CreatureAI(CreatureStateMachine creatureStateMachine)
    {
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckTarget();
    }

    private void CheckTarget() 
    {
        // 입력받은 각도에 따라 방향을 결정한다.
        bool isTarget = false;
        float halfAngle = visionRange / 2f;
        float rayToRay = visionRange / (rayAmount - 1);

        for (int i = 0; i < rayAmount; i++) 
        {
            // 현재 레이의 각도
            float currentAngle = - halfAngle + i * rayToRay;

            // 방향 벡터 계산 (로컬 좌표계 기준)
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            // 레이 발사
            if (Physics.Raycast(transform.position+(Vector3.up*0.5f), direction, out RaycastHit hit, visionDistance, player))
            {
                Debug.Log($"Hit: {hit.collider.name}");

                if (!visionInObject.Contains((int)SightInObject.Player))
                {
                    visionInObject.Add((int)SightInObject.Player); // ?? 내가 지정한게 int 인데 왜 형변환을 해야하지 ..?

                }
                isTarget = true;

                Debug.DrawLine(transform.position + (Vector3.up * 0.5f), hit.point, Color.red, 1.0f);
            }
            else
            {

                Debug.DrawRay(transform.position + (Vector3.up * 0.5f), direction * visionDistance, Color.green, 1.0f);
            }
        }

        CheckMissTime(isTarget);

    }

    private void CheckMissTime(bool isTarget)
    {
        if (!isTarget)
        {
            checkMissTime += Time.deltaTime;
            if (checkMissTime > missTargetTime)
            {
                Debug.Log("플레이어 놓침");
                isPlayerMiss = true;
            }
        }
        else
        {
            checkMissTime = 0;
        }
    }

    public void GetAggroGage(int amount)
    {
        aggroGage += amount;
    }

    
}

