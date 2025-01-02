using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : EnemyAI
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
}
