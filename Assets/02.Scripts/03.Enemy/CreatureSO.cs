using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName = "Creature")]
public class CreatureSO : ScriptableObject
{
    [field: SerializeField][field: Range(-10f, 10f)] public float PlayerChasingRange { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float AttackRange { get; private set; }
    //[field: SerializeField]public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }
    [field: SerializeField] public int Damage;
    [field: SerializeField][field: Range(-10f, 10f)] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float Dealing_End_TransitionTime { get; private set; }
}

