using UnityEngine;
internal class StateAttack : IState
{
    /// <summary>
    /// 이 상태를 관리하는 상태머신
    /// </summary>
    private EnemyStateMachine stateMachine;

    /// <summary>
    /// 실제 공격 처리를 위한 클래스
    /// </summary>
    private EnemyBattle battle;

    ///// <summary>
    ///// 공격 쿨타임
    ///// </summary>
    //float attackCoolTime = 0.0f;


    public StateAttack(EnemyStateMachine enemyStateMachine, EnemyBattle enemyBattle)
    {
        stateMachine = enemyStateMachine;
        battle = enemyBattle;
    }

    public void Enter()
    {
        Debug.Log("상태 진입 - Wait");
        stateMachine.Agent.isStopped = true;            // agent 정지
        stateMachine.Agent.velocity = Vector3.zero;     // agent에 남아있던 운동량(관성) 제거
        battle.ResetAttackCoolTime();                   // 공격 쿨타임 초기화
    }

    public void Exit()
    {
        Debug.Log("상태 나감 - Patrol");
    }

    public void Update()
    {
        IBattle attackTarget = stateMachine.PlayerInAttackRange();  // 공격 범위 안에 있는 플레이어 받아오기
        if (attackTarget != null)
        {
            battle.TryAttackAndLook(attackTarget);  // 공격 대상이 있으면 공격 시도
        }
        else
        {
            stateMachine.TransitionToChase();       // 공격 대상이 없으면 추적 상태로 전환
        }
    }
}