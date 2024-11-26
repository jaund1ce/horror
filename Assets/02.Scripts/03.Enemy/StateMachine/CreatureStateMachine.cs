using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStateMachine : MonoBehaviour
{
    public Creature Creature { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public GameObject Target { get; private set; }
    public CreatureIdleState IdleState { get; }
    public CreatureChasingState ChasingState { get; }
    public CreatureAttackState AttackState { get; }
    public CreatureStateMachine(Creature creature) 
    {
        this.Creature = creature;
        Target = GameObject.FindGameObjectWithTag("Player");

        IdleState = new CreatureIdleState(this);
        AttackState = new CreatureAttackState(this);
        ChasingState = new CreatureChasingState(this);


    }

}
