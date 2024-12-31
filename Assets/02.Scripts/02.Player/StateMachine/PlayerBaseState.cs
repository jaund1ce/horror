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
        input.PlayerActions.Movement.canceled += OnMovementCanceled;
        input.PlayerActions.Run.started += OnRunStarted;
        input.PlayerActions.Crouch.started += OnCrouchStarted;
        input.PlayerActions.JumpParkour.started += OnJumpStarted;
        input.PlayerActions.LightControl.started += OnLightControl;
    }


    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        input.PlayerActions.Run.started -= OnRunStarted;
        input.PlayerActions.Crouch.started -= OnCrouchStarted;
        input.PlayerActions.JumpParkour.started -= OnJumpStarted;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        if (!stateMachine.isAir)//�����ϱ� ���� �̵� ���� ������ �ش� ���� �����ϰ� ���������� �׷��� ������ ���ڸ� ������ ��
        {
            Move();
        }
    }

    public virtual void Update()//update�� ���� ���游�� ���ش�.
    {
        if (stateMachine.Player.PlayerRigidbody.velocity.y < -0.01f)//������ �ɾ� �ٴҶ� velocity.y�� ��Ȯ�� 0 �� �ƴϱ� ������ ������ �߻� �� �� ����
        {
            if (stateMachine.Player.isGround)
            {
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
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
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

        Vector3 rigidbodyChange = direction * movementSpeed;
        rigidbodyChange.y = stateMachine.Player.PlayerRigidbody.velocity.y;

        stateMachine.Player.PlayerRigidbody.velocity = rigidbodyChange;
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
