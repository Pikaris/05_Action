using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemyHealth))]
public class EnemyBattle : MonoBehaviour, IBattle
{
    /// <summary>
    /// 공격력
    /// </summary>
    [SerializeField]
    float attackPower = 10.0f;

    /// <summary>
    /// 방어력
    /// </summary>
    [SerializeField]
    float defencePower = 3.0f;

    /// <summary>
    /// 공격 인터벌(쿨타임)
    /// </summary>
    [SerializeField]
    float attackInterval = 1.0f;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    [SerializeField]
    float attackCoolTime = 0.0f;

    /// <summary>
    /// 적의 헬스(생존 여부 확인용)
    /// </summary>
    EnemyHealth enemyHealth;

    // 컴포넌트
    Animator animator;

    // 애니메이터용 해시
    readonly int Attack_Hash = Animator.StringToHash("Attack");
    readonly int Hit_Hash = Animator.StringToHash("Hit");

    /// <summary>
    /// 공격력 확인용 프로퍼티
    /// </summary>
    public float AttackPower => attackPower;

    /// <summary>
    /// 방어력 확인용 프로퍼티
    /// </summary>
    public float DefencePower => defencePower;


    /// <summary>
    /// 맞았음을 알리는 델리게이트(int : 실제로 입은 데미지에서 소수점을 제거한 값)
    /// </summary>
    public event Action<int> onHit;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 공격 함수
    /// </summary>
    /// <param name="target">공격 대상</param>
    public void Attack(IBattle target)
    {
        animator.SetTrigger(Attack_Hash);   // 공격 애니메이션
        target.Defense(AttackPower);        // 공격 대상에게 데미지 주기
    }

    /// <summary>
    /// 방어 함수
    /// </summary>
    /// <param name="damage">상대방이 가한 데미지</param>
    public void Defense(float damage)
    {
        if (enemyHealth.IsAlive)    // 살아있으면
        {
            animator.SetTrigger(Hit_Hash);      // 맞는 애니메이션

            float final = Mathf.Max(1, damage - defencePower);  // 최종 데미지 계산(최소 1, 단순하게 방어력만큼 빼는 방식)
            enemyHealth.GetDamage(final);           // 최종 데미지 적용
            onHit?.Invoke(Mathf.RoundToInt(final)); // 맞았다고 알림
        }
    }

    /// <summary>
    /// 공격 대상을 주시하고 공격 쿨타임이 다 되었으면 공격을 하는 함수(Update안에서 호출되어야 함)
    /// </summary>
    /// <param name="attackTarget">공격 대상</param>
    public void TryAttackAndLook(IBattle attackTarget)
    {
        attackCoolTime -= Time.deltaTime;       // 쿨타임 감소 시키고
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f); // 공격 대상쪽으로 회전
        if (attackCoolTime < 0.0f)              // 쿨타임이 다 되었으면
        {
            Attack(attackTarget);               // 공격
            ResetAttackCoolTime();              // 쿨타임 초기화
        }
    }

    /// <summary>
    /// 공격 쿨타임을 초기화하는 함수
    /// </summary>
    public void ResetAttackCoolTime()
    {
        attackCoolTime = attackInterval;
    }
}
