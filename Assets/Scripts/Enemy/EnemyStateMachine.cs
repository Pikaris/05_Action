using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(EnemyBattle))]
public class EnemyStateMachine : MonoBehaviour
{
    // 공용 변수 및 프로퍼티들 ---------------------------------------------------------------------------

    [Header("공용")]
    /// <summary>
    /// 이동 속도
    /// </summary>
    [SerializeField]
    public float moveSpeed = 3.0f;
    //---------------------------------------------------------------------------------------------------=

    // 대기 상태용 변수 및 프로퍼티들 ----------------------------------------------------------------------
    [Header("대기 상태용")]

    /// <summary>
    /// 대기 상태로 들어갔을 때 기다리는 시간
    /// </summary>
    [SerializeField]
    float waitTime = 1.0f;

    public float WaitTime => waitTime;

    //------------------------------------------------------------------------------------------------------



    // 순찰 상태용 변수 및 프로퍼티들 ----------------------------------------------------------------------
    [Header("순찰 상태용")]

    /// <summary>
    /// 웨이포인트
    /// </summary>
    [SerializeField]
    Waypoints waypoints;

    public Waypoints Waypoints => waypoints;
    //------------------------------------------------------------------------------------------------------

    [Header("공격 상태용")]

    /// <summary>
    /// 공격 상태로 들어가는 거리
    /// </summary>
    [SerializeField]
    public float attackRange = 1.5f;

    // 플레이어 탐색 용 변수들 -----------------------------------------------------------------------------

    /// <summary>
    /// 원거리 시야 범위
    /// </summary>
    [SerializeField]
    private float farSightRange = 10.0f;

    /// <summary>
    /// 원거리 시야각의 절반
    /// </summary>
    [SerializeField]
    private float sightHalfAngle = 60.0f;

    /// <summary>
    /// 근거리 시야 범위
    /// </summary>
    [SerializeField]
    private float nearSightRange = 1.5f;

    //------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 현재 상태
    /// </summary>
    IState state;

    // 전체 상태들
    StateWait wait;     // 대기 상태
    StatePatrol patrol; // 순찰 상태
    StateChase chase;   // 추적 상태
    StateAttack attack; // 공격 상태
    StateDie die;       // 사망 상태

    /// <summary>
    /// 각종 컴포넌트들 
    /// </summary>
    Animator animator;
    NavMeshAgent agent;

    /// <summary>
    /// 현재 상태를 확인하기 위한 프로퍼티
    /// </summary>
    public IState State => state;

    /// <summary>
    /// 현재 상태가 살아있는 상태인지 확인하는 프로퍼티(true면 살아있는 상태, false면 Die 상태)
    /// </summary>
    public bool IsAliveState => state != die;

    // 컴포넌트들에 접근하기 위한 프로퍼티들
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
    }

    void Start()
    {
        wait = new StateWait(this);
        patrol = new StatePatrol(this);
        chase = new StateChase(this);
        attack = new StateAttack(this, GetComponent<EnemyBattle>());    // StateAttack은 상태머신과 EnemyBattle을 받음
        die = new StateDie(this);

        state = wait;       // 대기 상태를 현재 상태로 지정
    }

    private void Update()
    {
        state.Update();     // 현재 상태의 Update 함수 실행
    }

    /// <summary>
    /// 현재 상태를 다른 상태로 전이 시키는 함수
    /// </summary>
    /// <param name="target">전이할 상태</param>
    private void TransitionTo(IState target)
    {
        if (target != null)
        {
            target.Exit();      // 이전 상태의 Exit 실행
            state = target;     // 상태를 target으로 변경
            state.Enter();      // 새 상태의 Enter 실행
        }
    }

    /// <summary>
    /// 현재 상태를 대기 상태로 전이시키는 함수
    /// </summary>
    public void TransitionToWait()
    {
        TransitionTo(wait);
    }

    /// <summary>
    /// 현재 상태를 순찰 상태로 전이시키는 함수
    /// </summary>
    public void TransitionToPatrol()
    {
        TransitionTo(patrol);
    }

    /// <summary>
    /// 현재 상태를 추적 상태로 전이시키는 함수
    /// </summary>
    public void TransitionToChase()
    {
        TransitionTo(chase);
    }

    /// <summary>
    /// 현재 상태를 공격 상태로 전이시키는 함수
    /// </summary>
    public void transitionToAttack()
    {
        TransitionTo(attack);
    }

    /// <summary>
    /// 현재 상태를 사망 상태로 전이시키는 함수
    /// </summary>
    public void transsitionToDie()
    {
        TransitionTo(die);
    }

    /// <summary>
    /// 플레이어를 탐색하는 함수
    /// </summary>
    /// <param name="potiion">플레이어의 위치(발견했을 때)</param>
    /// <returns>true면 발견했다, false면 발견하지 못했다</returns>
    public bool SearchPlayer(out Vector3 position)
    {
        bool result = false;
        position = Vector3.zero;

        // farSightRange안에 있는 Player 레이어인 컬라이더 모두 찾기(1개만 있음)
        Collider[] colliders = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)    // 찾았으면
        {
            IHealth health = colliders[0].GetComponent<IHealth>();  // IHealth 인터페이스로 가져오고
            if (health != null && health.IsAlive)   // 살아있을 때만 처리
            {
                // 원거리 범위 안에 플레이어가 살아있는 상태로 있다

                Vector3 playerPos = colliders[0].transform.position;    // 플레이어 위치 찾기
                Vector3 toPlayerDir = playerPos - transform.position;   // 슬라임 -> 플레이어로 가는 방향 벡터
                if (toPlayerDir.sqrMagnitude < nearSightRange * nearSightRange)
                {
                    // 근거리 범위에 플레이어가 있다
                    position = playerPos;
                    result = true;
                }
                else
                {
                    // (근거리 범위 밖 ~ 원거리 범위 안) 사이에 플레이어가 있다
                    if (IsInSightAngle(toPlayerDir))    // 시야각 안에 있는지 확인
                    {
                        if (IsSightClear(toPlayerDir))   // 시야각 안에 있으면 중간에 가리는 물체가 있는지 확인
                        {
                            position = playerPos;
                            result = true;
                        }
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 시야각 안에 플레이어가 있는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">슬라임이 플레이어를 바라보는 방향 벡터</param>
    /// <returns>시야각 안에 있으면 true, 없으면 false</returns>
    bool IsInSightAngle(Vector3 toTargetDir)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDir);

        return sightHalfAngle > angle;  // sightHalfAngle보다 angle이 작아야 시야각 안이다
    }


    /// <summary>
    /// 플레이어가 다른 오브젝트에 의해 가려지는지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">슬라임이 플레이어를 바라보는 방향 벡터</param>
    /// <returns>true면 슬라임과 플레이어 사이에 다른 오브젝트가 없다. false면 다른 오브젝트가 가리고 있다</returns>
    bool IsSightClear(Vector3 toTargetDir)
    {
        bool result = false;
        Ray ray = new Ray(transform.position + transform.up * 0.5f, toTargetDir);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, farSightRange))
        {
            //hitInfo.collider.gameObject.layer == LayerMask.GetMask("Player")
            if (hitInfo.collider.CompareTag("Player"))   // 처음 레이와 닿은 것이 플레이어이면
            {
                result = true;  // 가리는 물체가 없다
            }
        }
        return result;
    }

    /// <summary>
    /// 공격 범위 안에 플레이어가 있는지 확인하는 함수
    /// </summary>
    /// <returns>null이 아니면 공격범위안에 플레이어가 있다, null이면 없다</returns>
    public IBattle PlayerInAttackRange()
    {
        IBattle attackTarget = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Player"));     // Player 레이어 확인
        if (colliders.Length > 0)
        {
            attackTarget = colliders[0].GetComponent<IBattle>();    // IBattle 가져오기(없으면 null)
        }
        return attackTarget;
    }

    /// <summary>
    /// 사망 연출용 코루틴 실행 시키는 함수
    /// </summary>
    public void StartDieCoroutine()
    {
        StartCoroutine(die.DieCoroutine());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        bool isPlayerShow = SearchPlayer(out Vector3 _);
        Handles.color = isPlayerShow ? Color.red : Color.blue;
        Handles.DrawWireDisc(transform.position, transform.up, nearSightRange, 5.0f);       // 근거리 시야 범위 표시

        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);     // 원거리 중심선 그리기

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward, 3.0f);      // 부채꼴의 왼쪽 직선

        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q2 * forward, 3.0f);      // 부채꼴의 오른쪽 직선

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2.0f, farSightRange, 3.0f);
    }


#endif
}
