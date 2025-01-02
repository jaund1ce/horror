using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzyer : Enemy
{

    protected override void Awake()
    {
        base.Awake();
        EnemyAI = GetComponent<FrenzyerAI>();
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
