using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField]
    float hp = 100.0f;

    [SerializeField]
    float maxHP = 100.0f;

    EnemyStateMachine stateMachine;

    public float HP
    {
        get => hp;
        set
        {
            if (stateMachine.IsAliveState)
            {
                hp = value;
                if (hp <= 0)
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0, maxHP);
                onHealthChange?.Invoke(hp / maxHP);
            }
        }
    }

    public float MaxHP => maxHP;

    public bool IsAlive => hp > 0;

    public event Action<float> onHealthChange;
    public event Action onDie;

    

    void Awake()
    {
        stateMachine = GetComponent<EnemyStateMachine>();
    }

    public void Die()
    {
        Debug.Log("슬라임 사망");

        stateMachine.transsitionToDie();
        onDie?.Invoke();
    }

    /// <summary>
    /// 데미지를 입는 함수
    /// </summary>
    /// <param name="damage">입은 데미지 양</param>
    public void GetDamage(float damage)
    {
        HP -= damage;
    }

    public void HealthHeal(float heal)
    {
        HP += heal;     // 테스트용
    }

    public void HealthRegenerate(float totalRegen, float duration)
    {
        throw new NotImplementedException();    // 사용 안함
    }

    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        throw new NotImplementedException();    // 사용 안함(이대로 두면 터짐)
    }
}
