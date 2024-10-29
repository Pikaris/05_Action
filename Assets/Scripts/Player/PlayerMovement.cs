using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 5.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 15.0f;

    /// <summary>
    /// 회전 정도(10이면 0.1초 정도)
    /// </summary>
    public float turnSmooth = 10.0f;

    /// <summary>
    /// 현재 이동 속도
    /// </summary>
    float currentSpeed = 1.0f;

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveState currentMoveMode = MoveState.Run;

    /// <summary>
    /// 이동할 방향
    /// </summary>
    Vector3 direction = Vector3.zero;

    /// <summary>
    /// 목표로 하는 회전
    /// </summary>
    Quaternion targetRotation;

    bool movePressed = false;

    /// <summary>
    /// 현재 이동 상태를 확인하기 위한 프로퍼티
    /// </summary>
    public MoveState MoveMode => currentMoveMode;

    /// <summary>
    /// 이동할 방향을 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    public Vector3 Direction
    {
        get => direction;
        set
        {
            direction = value;
        }
    }

    /// <summary>
    /// 이동 상태 표시용 enum
    /// </summary>
    public enum MoveState : byte
    {
        Stop,   // 정지 상태
        Walk,   // 걷기 상태
        Run,    // 달리기 상태
    }

    // 애니메이터용 해시값 및 상수
    readonly int Speed_Hash = Animator.StringToHash("Speed");
    const float Animator_StopSpeed = 0.0f;
    const float Animator_WalkSpeed = 0.3f;
    const float Animator_RunSpeed = 1.0f;

    // 컴포넌트
    CharacterController controller;
    Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetMoveSpeedAndAnimation(MoveState.Stop);    // 일단 정지 상태
    }

    private void Update()
    {
        //characterController.Move(Time.deltaTime * inputDirection);      // 수동
        //characterController.SimpleMove(inputDirection);                 // 자동

        // 1. W를 누르면 카메라 앞쪽으로 플레이어의 forward를 회전 시키고 그 방향으로 전진한다
        // 1. A를 누르면 카메라 왼쪽으로 플레이어의 forward를 회전 시키고 그 방향으로 전진한다
        // 1. S를 누르면 카메라 뒤쪽으로 플레이어의 forward를 회전 시키고 그 방향으로 전진한다
        // 1. D를 누르면 카메라 오른쪽으로 플레이어의 forward를 회전 시키고 그 방향으로 전진한다

        controller.Move(Time.deltaTime * direction * currentSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSmooth);
        //MoveSpeedChange(currentMoveMode);
    }

    /// <summary>
    /// 이동할 방향을 지정하는 함수
    /// </summary>
    /// <param name="input">입력된 방향</param>
    /// <param name="isPress">키를 눌렀으면 true, 땠으면 false</param>
    public void SetDirection(Vector2 input, bool isPress)
    {
        direction.x = input.x;
        direction.y = 0.0f;
        direction.z = input.y;
        movePressed = isPress;

        if (isPress)
        {
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);     // 카메라의 Y축 회전만 따로 추출
            direction = camY * direction;                           // direction 방향을 camY만큼 회전 시키기
            targetRotation = Quaternion.LookRotation(direction);    // 목표로 하는 회전 설정

            SetMoveSpeedAndAnimation(currentMoveMode);              // 현재 모드에 맞게 이동 속도와 애니메이션 설정
        }
        else
        {
            SetMoveSpeedAndAnimation(MoveState.Stop);    // 정지
        }

        direction = direction.normalized;               // 정규화 시키기
    }

    /// <summary>
    /// 걷기와 달리기를 서로 토글하는 함수(이동 모드 변화 키가 눌려질 때 실행)
    /// </summary>
    public void ToggleMoveMode()
    {
        switch (currentMoveMode)
        {
            case MoveState.Walk:
                SetMoveSpeedAndAnimation(MoveState.Run);    // 속도와 애니메이션 변화
                currentMoveMode = MoveState.Run;            // 상태 변화
                break;
            case MoveState.Run:
                SetMoveSpeedAndAnimation(MoveState.Walk);
                currentMoveMode = MoveState.Walk;
                break;
        }
        if (!movePressed)
        {
            SetMoveSpeedAndAnimation(MoveState.Stop);
        }
    }

    /// <summary>
    /// 플레이어 이동 속도 변화와 애니메이션 처리용 함수
    /// </summary>
    /// <param name="mode">설정할 이동 모드</param>
    void SetMoveSpeedAndAnimation(MoveState mode)
    {
        // 속도와 애니메이션 변경
        switch(mode)
        {
            case MoveState.Stop:
                currentSpeed = 0.0f;
                animator.SetFloat(Speed_Hash, Animator_StopSpeed);
                break;
            case MoveState.Walk:
                //if (speed > 0)      // 이동 중이면 애니메이션 실행
                {
                    animator.SetFloat(Speed_Hash, Animator_WalkSpeed);
                }
                currentSpeed = walkSpeed;
                break;
            case MoveState.Run:
                //if (speed > 0)
                {
                    animator.SetFloat(Speed_Hash, Animator_RunSpeed);
                }
                currentSpeed = runSpeed;
                break;
        }
    }
}

// 1. 이동 모드를 변경하면 애니메이션이 변경되고 이동 속도도 변경되어야 함
// 2. 토글버튼을 눌렀을 때 이동 모드가 변경되어야 한다.(이동 중에도 적용되어야 함)
// 3. 정지 중에 토글 버튼을 눌러도 idle 애니메이션이 나와야 한다.
