using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerMovement))]
public class PlayerBattle : MonoBehaviour, IBattle
{
    /// <summary>
    /// 공격 애니메이션 재생 시간(공통)
    /// </summary>
    const float AttackAnimLength = 0.533f;

    /// <summary>
    /// 쿨타임 설정용 변수(콤보를 위해서 애니메이션 시간보다 작아야한다)
    /// </summary>
    [Range(0, AttackAnimLength)]
    [SerializeField]
    public float maxCoolTime = 0.3f;

    [SerializeField]
    private float attackPower = 10.0f;

    [SerializeField]
    private float defencePower = 5.0f;


    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float coolTime = 0.0f;

    // 컴포넌트
    Animator animator;
    PlayerMovement playerMovement;

    // 애니메이션용 해시
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    public event Action<int> onHit;

    public float AttackPower => attackPower;

    public float DefencePower => defencePower;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        coolTime -= Time.deltaTime;
    }

    /// <summary>
    /// 공격 입력이 들어오면 실행되는 함수
    /// </summary>
    public void OnAttackInput()
    {
        Attack(null);
    }


    /// <summary>
    /// 공격 한번을 하는 함수(일단 모션만 구현)
    /// </summary>
    /// <param name="target"></param>
    public void Attack(IBattle target)
    {
        // 쿨타임이 다 되거나, 가만히 서 있거나, 걷기 모드일때만 공격 가능
        if (coolTime < 0 && playerMovement.MoveMode != PlayerMovement.MoveState.Run)
        {
            animator.SetTrigger(Attack_Hash);
            coolTime = maxCoolTime;
        }
    }

    /// <summary>
    /// 방어용 함수(형태만 구현)
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    public void Defense(float damage)
    {
        onHit?.Invoke((int)damage);
        Debug.Log($"{damage}의 피해를 받았다.");
    }
}
