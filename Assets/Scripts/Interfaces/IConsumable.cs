using UnityEngine;

/// <summary>
/// 아이템 중 획득시 즉시 소비되는 아이템에 추가할 인터페이스
/// </summary>
public interface IConsumable
{
    /// <summary>
    /// 아이템을 소비시키는 인터페이스
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    void Consume(GameObject target);
}