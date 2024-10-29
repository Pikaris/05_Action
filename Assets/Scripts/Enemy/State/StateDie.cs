using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

internal class StateDie : IState
{
    /// <summary>
    /// 이 상태를 관리하는 상태머신
    /// </summary>
    private EnemyStateMachine stateMachine;

    // 애니메이터용 해시
    readonly int Die_Hash = Animator.StringToHash("Die");

    public StateDie(EnemyStateMachine enemyStateMachine)
    {
        stateMachine = enemyStateMachine;
    }

    public void Enter()
    {
        Debug.Log("상태 진입 - Die");
        stateMachine.Agent.isStopped = true;            // agent 정지
        stateMachine.Agent.velocity = Vector3.zero;     // agent에 남아있던 운동량(관성) 제거
        stateMachine.Animator.SetTrigger(Die_Hash);
        stateMachine.StartDieCoroutine();
    }

    public void Exit()
    {
        Debug.Log("상태 나감 - Die");
    }

    public void Update()
    {

    }

    

    public IEnumerator DieCoroutine()
    {
        GameObject obj = stateMachine.gameObject;
        Transform child;

        Collider col = obj.GetComponent<Collider>();
        col.enabled = false;                    // 컬라이더 끄기. 더 이상 맞지 않는다

        
        child = obj.transform.GetChild(5);
        child.SetParent(null);
        GameObject effect = child.gameObject;
        effect.gameObject.SetActive(true);       // 사망용 이펙트 보이기

        child = obj.transform.GetChild(3);
        child.gameObject.SetActive(false);      // HP바 끄기
        

        yield return new WaitForSeconds(0.5f);  // 잠깐 기다리기(아이템 드랍되는 타이밍까지 대기)

        // 아이템 드랍
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.DropItems();
        
        yield return new WaitForSeconds(1.0f);  // 사망 애니메이션 끝날 때까지 대기(시작 후 1.5초가 되는 타이밍. 사망 애니메이션 시간 1.333초)

        // 바닥으로 가라앉기 시작
        stateMachine.Agent.enabled = false; // NavMeshAgent 컴포넌트 비활성화
        Rigidbody rigid = obj.GetComponent<Rigidbody>();
        rigid.drag = 10.0f;             // 마찰력 무한에서 낮추기
        rigid.isKinematic = false;      // 키네마틱 끄기


        // 충분히 가라 앉을 때까지 대기
        yield return new WaitForSeconds(2.0f);

        // 삭제
        GameObject.Destroy(obj);
        GameObject.Destroy(effect);

        yield return null;
    }
}