using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStateMachine : StateMachine
{
    public Creature Creature { get; set; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }

    public GameObject Target { get; private set; }
    
    public CreatureIdleState IdleState { get; }
    public CreatureChasingState ChasingState { get; }
    public CreatureAttackState AttackState { get; }
    public CreatureWanderState WanderState { get; }
    public CreatureStateMachine(Creature creature) 
    {
        this.Creature = creature;
        Target = GameObject.FindGameObjectWithTag("Player");

        IdleState = new CreatureIdleState(this);
        AttackState = new CreatureAttackState(this);
        ChasingState = new CreatureChasingState(this);
        WanderState = new CreatureWanderState(this);

        RotationDamping = Creature.Data.GroundData.BaseRotationDamping;
    }

}
