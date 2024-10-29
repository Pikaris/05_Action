using System;
using UnityEngine;

/// <summary>
/// 전투가 가능한 것을 알리는 인터페이스
/// </summary>
public interface IBattle
{
    /// <summary>
    /// 이 오브젝트의 트랜스폼
    /// </summary>
    Transform transform { get; }

    /// <summary>
    /// 공격력 확인용 프로퍼티
    /// </summary>
    float AttackPower { get; }

    /// <summary>
    /// 방어력 확인용 프로퍼티
    /// </summary>
    float DefencePower { get; }

    /// <summary>
    /// 맞았음을 알리는 델리게이트(int : 실제로 입은 데미지에서 소수점을 제거한 값)
    /// </summary>
    event Action<int> onHit;

    /// <summary>
    /// 공격 함수
    /// </summary>
    /// <param name="target">공격하는 대상</param>
    void Attack(IBattle target);

    /// <summary>
    /// 방어 함수
    /// </summary>
    /// <param name="damage">적이 나에게 준 데미지</param>
    void Defense(float damage);
}