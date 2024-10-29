using UnityEngine;

public class StatePatrol : IState
{
    /// <summary>
    /// 이 상태를 관리하는 상태머신
    /// </summary>
    private EnemyStateMachine stateMachine;

    // 애니메이터용 해시
    readonly int Move_Hash = Animator.StringToHash("Move");

    public StatePatrol(EnemyStateMachine enemyStateMachine)
    {
        stateMachine = enemyStateMachine;
    }

    public void Enter()
    {
        Debug.Log("상태 진입 - Patrol");
        stateMachine.Agent.isStopped = false;           // agent 다시 켜기
        stateMachine.Agent.SetDestination(stateMachine.Waypoints.NextTarget);   // 다음 이동 지점 설정
        stateMachine.Animator.SetTrigger(Move_Hash);    // 이동 애니메이션 재생
    }

    public void Exit()
    {
        Debug.Log("상태 나감 - Patrol");
    }

    public void Update()
    {
        if (stateMachine.SearchPlayer(out Vector3 _))
        {
            // 플레이어를 찾았으면 추적 상태로 전이
            stateMachine.TransitionToChase();
        }
        else
        {
            // 플레이어를 못 찾았으면 계속 순찰
            if(stateMachine.Agent.remainingDistance <= stateMachine.Agent.stoppingDistance)
            {
                stateMachine.Waypoints.StepNextWaypoint();
                stateMachine.TransitionToWait();
            }
        }
    }
}