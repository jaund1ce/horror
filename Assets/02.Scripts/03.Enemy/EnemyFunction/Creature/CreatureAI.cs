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



public class CreatureAI : EnemyAI
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckTarget()
    {
        base.CheckTarget();
    }

    protected override void CheckMissTime(bool isTarget)
    {
        base.CheckMissTime(isTarget);
    }

    public override void GetAggroGage(float amount)
    {
        base.GetAggroGage(amount);
    }

    public override void FeelThePlayer() 
    {
        base.FeelThePlayer();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    protected override bool IsInAttackRange()
    {
        return base.IsInAttackRange();
    }

    public override int UpdateState()
    {
        return base.UpdateState();
    }
    protected override void PlaySoundBasedOnState()
    {
        base.PlaySoundBasedOnState();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

}

