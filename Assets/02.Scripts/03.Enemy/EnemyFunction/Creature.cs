using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Creature : Enemy
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        EnemyAI = GetComponent<CreatureAI>();
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
       base .FixedUpdate();
    }
}
