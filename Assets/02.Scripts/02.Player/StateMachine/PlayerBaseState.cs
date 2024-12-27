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
    protected bool isRunnig;
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
        input.playerActions.Crouch.canceled += OnCrouchCanceled;
        input.playerActions.JumpParkour.started += OnJumpStarted;
        input.playerActions.LightControl.started += OnLightControl;
    }


    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Crouch.started -= OnCrouchStarted;
        input.playerActions.Crouch.canceled -= OnCrouchCanceled;
        input.playerActions.JumpParkour.started -= OnJumpStarted;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()//언제든 떨어지면 하강 상태로 변경, 플레이어에서 호출 중이기 때문에 특별한 작업의 경우 base.physicupdate는 불필요하다./ 하지만 우리는 어떤상태이던지 낙하 상태가 필요하기 때문에 상속이 필요하다.
    {
        if (stateMachine.Player.PlayerRigidbody.velocity.y < 0)
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

    public virtual void Update()
    {
        Move();
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
        isRunnig = true;
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (!stateMachine.Player.isGround) return;
    }

    protected virtual void OnCrouchStarted(InputAction.CallbackContext context)
    {
        stateMachine.isCrouching = true;
    }

    protected virtual void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        stateMachine.isCrouching = false;
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

        Move(movementDirection);
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
        stateMachine.Player.PlayerRigidbody.velocity = direction * movementSpeed;
    }

    private float GetMovementSpeed()
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
