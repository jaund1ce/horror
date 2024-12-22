using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; set; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }

    public GameObject Target { get; private set; }
    
    public EnemyIdleState IdleState { get; }
    public EnemyChasingState ChasingState { get; }
    public EnemyAttackState AttackState { get; }
    public EnemyWanderState WanderState { get; }
    public EnemyStateMachine(Enemy Enemy) 
    {
        this.Enemy = Enemy;
        Target = GameObject.FindGameObjectWithTag("Player");

        IdleState = new EnemyIdleState(this);
        AttackState = new EnemyAttackState(this);
        ChasingState = new EnemyChasingState(this);
        WanderState = new EnemyWanderState(this);

        RotationDamping = Enemy.Data.GroundData.BaseRotationDamping;
    }

}
