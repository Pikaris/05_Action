
using UnityEngine;

/// <summary>
/// 대기 상태. 
/// 1. 상태에 진입하면 지정된 시간동안 대기
/// 2. 대기 중에는 두리번 거리는 애니메이션 재생
/// 3. 대기 시간이 끝나면 StatePatrol 상태로 전이
/// 4. 대기 시간 중에 플레이어를 발견하면 StateChase 상태로 전이
/// </summary>                                                                                                                                                                  
public class StateWait : IState
{
    /// <summary>
    /// 이 상태를 관리하는 상태머신
    /// </summary>
    EnemyStateMachine stateMachine;

    /// <summary>
    /// 순찰 상태로 전이하기까지 남은 시간
    /// </summary>
    float waitCountDown;

    // 애니메이터용 해시
    readonly int Stop_Hash = Animator.StringToHash("Stop");

    public StateWait(EnemyStateMachine enemyStateMachine)
    {
        stateMachine = enemyStateMachine;
        waitCountDown = stateMachine.WaitTime;
    }

    public void Enter()
    {
        Debug.Log("상태 진입 - Wait");
        waitCountDown = stateMachine.WaitTime;          // 대기 시간 초기화
        stateMachine.Agent.isStopped = true;            // agent 정지
        stateMachine.Agent.velocity = Vector3.zero;     // agent에 남아있던 운동량(관성) 제거
        stateMachine.Animator.SetTrigger(Stop_Hash);    // 두리번 거리는 애니메이션 재생
    }

    public void Exit()
    {
        Debug.Log("상태 나감 - Wait");
    }

    public void Update()
    {
        if(stateMachine.SearchPlayer(out Vector3 _))
        {
            // 플레이어를 찾았으면 추적 상태로 전이
            stateMachine.TransitionToChase();
        }
        else
        {
            // 플레이어를 못 찾았으면 계속 대기

            waitCountDown -= Time.deltaTime;        // 매 프레임마다 시간 감소
            if (waitCountDown < 0)                  // 시간이 0이하로 내려가면
            {
                stateMachine.TransitionToPatrol();  // 순찰 상태로 전이
            }
        }
    }
}
