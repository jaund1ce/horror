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


public class CreatureAI : MonoBehaviour , AggroGage
{

    public enum SightInObject 
    {
        Player = 0,
        Object = 1
    }

    public LayerMask player;
    private NavMeshAgent agent;
    [field: SerializeField] public CreatureSO Data { get; private set; }


    public bool isPlayerMiss { get; private set; }
    public bool IsAggroGageMax { get; private set; }
    private float checkMissTime;
    private float aggroGage;
    
    private List<int> visionInObject;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        visionInObject = new List<int>();
    }

    private void Update()
    {
        CheckTarget();
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

        if (aggroGage >= Data.MaxAggroGage) 
        {
            IsAggroGageMax = true;
        }
        else 
        {
            IsAggroGageMax = false;
        }
    }


}

