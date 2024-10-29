using System;
using UnityEngine.InputSystem.Android;

internal interface IMana
{
    /// <summary>
    /// MP 확인용 프로퍼티
    /// </summary>
    float MP { get; }
    
    /// <summary>
    /// 최대 MP 확인용 프로퍼티
    /// </summary>
    float MaxMP { get; }

    /// <summary>
    /// MP가 변경될 때마다 실행될 델리게이트(float:변경된 비율)
    /// </summary>
    event Action<float> onManaChange;

    /// <summary>
    /// 마나를 지속적으로 증가시켜 주는 함수, 초당 totalRegen/duration만큼 회복
    /// </summary>
    /// <param name="totalRegen"></param>
    /// <param name="duration"></param>
    void ManaRegenerate(float totalRegen, float duration);

    /// <summary>
    /// MP를 틱단위로 회복시켜주는 함수
    /// </summary>
    /// <param name="tickRegen">틱 당 회복량</param>
    /// <param name="tickInterval">틱 간의 시간 간격</param>
    /// <param name="totalTickCount">전체 틱 수</param>
    void ManaRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount);

    /// <summary>
    /// MP를 한번에 회복시키는 함수
    /// </summary>
    /// <param name="restore">회복량</param>
    void ManaRestore(float restore);
}