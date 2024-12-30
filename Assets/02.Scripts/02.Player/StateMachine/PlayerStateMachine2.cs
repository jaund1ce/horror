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
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }

    //���࿡ ��� state �� �����ؾ��ϴ� ������ �ִٸ� ���⿡ �����ؾ��Ѵ�.
    public bool isCrouching;
    public bool isAir;

    public PlayerStateMachine2(Player player)//�����ϴ� ��� ������Ʈ ���¸� ������ �����
    {
        this.Player = player;
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);

        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        
        CrouchIdleState = new PlayerCrouchIdleState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
        JumpForce = player.Data.AirData.JumpForce;
    }

}
