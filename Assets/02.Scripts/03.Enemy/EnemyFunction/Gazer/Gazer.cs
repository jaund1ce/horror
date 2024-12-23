using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gazer : Enemy
{

    protected override void Awake()
    {
        base.Awake();
        EnemyAI = GetComponent<GazerAI>();
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
