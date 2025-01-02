using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        CharacterController.areaMask = NavMesh.AllAreas;
        CharacterController.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        EnemyAI = GetComponent<GhostAI>();
        stateMachine = new EnemyStateMachine(this);
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
