using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine2 stateMachine;
    protected readonly PlayerGroundData groundData;
    private bool IsLightOn;
    public Light light;

    public PlayerBaseState(PlayerStateMachine2 stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
        light = stateMachine.Player.gameObject.GetComponentInChildren<Light>(true);
    }
    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    protected virtual void AddInputActionCallbacks()
    {   
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Crouch.started += OnCrouchStarted;
        input.playerActions.JumpParkour.started += OnJumpStarted;
        input.playerActions.LightControl.started += OnLightControl;
    }


    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Crouch.started -= OnCrouchStarted;
        input.playerActions.JumpParkour.started -= OnJumpStarted;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()//언제든 떨어지면 하강 상태로 변경, 플레이어에서 호출 중이기 때문에 특별한 작업의 경우 base.physicupdate는 불필요하다./ 하지만 우리는 어떤상태이던지 낙하 상태가 필요하기 때문에 상속이 필요하다.
    {
        if (!stateMachine.isAir)//점프하기 전에 이동 중이 었으면 해당 값을 유지하고 점프하지만 그렇지 ㅇ낳으면 제자리 점프만 함
        {
            Move();
        }
    }

    public virtual void Update()//update는 값의 변경만을 해준다.
    {
        //Debug.Log($"{stateMachine.Player.PlayerRigidbody.velocity.y}");
        if (stateMachine.Player.PlayerRigidbody.velocity.y < -0.01f)
        {
            if (stateMachine.Player.isGround)
            {
                if (stateMachine.isCrouching) { stateMachine.ChangeState(stateMachine.CrouchIdleState); return; }//없으면 왜인지 모르나 지상에서 가끔씩 상태가 idle로 감

                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }

            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.isCrouching)
        {
            stateMachine.ChangeState(stateMachine.CrouchIdleState);
        }
        else if (stateMachine.Player.isGround)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (!stateMachine.Player.isGround) return;
    }

    protected virtual void OnCrouchStarted(InputAction.CallbackContext context)
    {
        
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    private void Move() 
    {
        Vector3 movementDirection = GetMovementDirection(); 

        Move(movementDirection); //이부분이 문제....
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        Vector3 dir = direction * movementSpeed;
        dir.y = stateMachine.Player.PlayerRigidbody.velocity.y;

        stateMachine.Player.PlayerRigidbody.velocity = dir; //아마도 여기가 문제?
    }

    private float GetMovementSpeed()//매번 다른 스테이트에 들어갈때 마다 바꿔주니 상관 x
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }
    private void OnLightControl(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsLightOn)
            {
                light.gameObject.SetActive(false);
                IsLightOn = false;
                Debug.Log("isLightOff");
            }
            else if (!IsLightOn)
            {
                light.gameObject.SetActive(true);
                IsLightOn = true;
                Debug.Log("isLightOn");
            }
        }
    }

    
}
