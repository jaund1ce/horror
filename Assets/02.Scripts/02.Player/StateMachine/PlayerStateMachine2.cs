using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine2 : StateMachine
{ 
    public Player Player { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed {  get; private set; }
    public float RotationDamping {  get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float JumpForce { get; set; }
    public Transform MainCamTransform { get; set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    public PlayerStateMachine2(Player player)//존재하는 모든 스테이트 상태를 선언해 줘야함
    {
        this.Player = player;
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);

        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        
        CrouchState = new PlayerCrouchState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }

}
