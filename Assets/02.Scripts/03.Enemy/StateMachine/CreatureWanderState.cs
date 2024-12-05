using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CreatureWanderState : CreatureBaseState
{
    public CreatureWanderState(CreatureStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        StartAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StartAnimation(stateMachine.Creature.AnimationData.WalkParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Creature.AnimationData.GrondParameterHash);
        StopAnimation(stateMachine.Creature.AnimationData.WalkParameterHash);

    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Creature.CreatureAI.CreatureAistate == AIState.Chasing)
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }

    }
}

