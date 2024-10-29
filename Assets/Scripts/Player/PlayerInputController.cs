using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 이동 입력을 전달하는 델리게이트(Vector2 : 이동 방향, bool : 누른 상황(true면 눌렀다))
    /// </summary>
    public event Action<Vector2, bool> onMove;

    /// <summary>
    /// 이동 모드 변경 입력을 전달하는 델리게이트
    /// </summary>
    public event Action onMoveModeChange;

    /// <summary>
    /// 공격 입력을 전달하는 델리게이트
    /// </summary>
    public event Action onAttack;

    /// <summary>
    /// 아이템을 줍는 입력을 전달하는 델리게이트
    /// </summary>
    public event Action onPickUp;

    // 인풋 액션 에셋
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.PickUp.performed += OnPickUp;
    }


    private void OnDisable()
    {
        inputActions.Player.PickUp.performed -= OnPickUp;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);
    }

    private void OnMoveModeChange(InputAction.CallbackContext _)
    {
        onMoveModeChange?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        onAttack?.Invoke();
    }

    private void OnPickUp(InputAction.CallbackContext _)
    {
        onPickUp?.Invoke();
    }
}
