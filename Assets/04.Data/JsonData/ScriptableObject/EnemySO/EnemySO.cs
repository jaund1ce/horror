using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName = "Characters/Creature")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }
    [field: SerializeField] public int Damage;
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }

    [field: Header("CreatureSight")]
    [field: SerializeField][field: Range(30f, 180f)] public float VisionRange { get; private set; }
    [field: SerializeField][field: Range(0f, 20f)] public float VisionDistance { get; private set; }
    [field: SerializeField][field: Range(10f, 40f)] public int RayAmount { get; private set; }
    [field: SerializeField][field: Range(0f, 20f)] public int FeelPlayerRange { get; private set; }

    [field: Header("CreatureAI")]
    [field: SerializeField][field: Range(0f, 5f)] public float MissTargetTime { get; private set; }
    [field: SerializeField][field: Range(0f, 100f)] public float MaxAggroGage { get; private set; }
    [field: SerializeField][field: Range(0f, 20f)] public float MinWanderDistance { get; private set; }
    [field: SerializeField][field: Range(0f, 20f)] public float MaxWanderDistance { get; private set; }
}

