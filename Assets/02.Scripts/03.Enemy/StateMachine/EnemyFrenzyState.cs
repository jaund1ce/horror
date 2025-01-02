using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class EnemyFrenzyState : EnemyBaseState
{
    private float crawlTime = 0.5f;
    private float frenzyTime = 1.5f;
    private float checkTime;
    public EnemyFrenzyState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MovementSpeedModifier = groundData.SpecialMovementModifier;
        StartAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.FrenzyParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        MovementSpeedModifier = groundData.WalkSpeedModifier;
        StopAnimation(stateMachine.Enemy.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.FrenzyParameterHash);


    }

    public override void Update()
    {
        checkTime += Time.deltaTime;
        if (checkTime < crawlTime)
        {
            MovementSpeedModifier = 0;
        }
        else if (checkTime >= crawlTime && checkTime <= frenzyTime)
        {
            MovementSpeedModifier = groundData.SpecialMovementModifier;
        }
        else checkTime = 0f;
        base.Update();
    }

}

