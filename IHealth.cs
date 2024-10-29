public interface IHealth
{
    /// <summary>
    /// HP 확인용 프로퍼티
    /// </summary>
    float HP { get; }

    /// <summary>
    /// 최대 HP 확인용 프로퍼티
    /// </summary>
    float MaxHP {  get; }

    /// <summary>
    /// 생존 확인용 프로퍼티
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// HP 변화를 알리는 델리게이트(float : 변화 비율)
    /// </summary>
    event Action<float> onHealthChange;

    /// <summary>
    /// 사망을 알리는 델리게이트
    /// </summary>
    event Action onDie;

    /// <summary>
    /// HP를 지속적으로 회복시키는 함수
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">회복 기간</param>
    void HealthRegenerate(float totalRegen, float duration);

    /// <summary>
    /// HP를 틱 단위로 회복시키는 함수
    /// </summary>
    /// <param name="tickRegen">틱 당 회복량</param>
    /// <param name="tickInterval">틱 당 간격</param>
    /// <param name="totalTickCount">전체 틱 수</param>
    void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount);

    /// <summary>
    /// HP를 즉시 회복시키는 함수
    /// </summary>
    /// <param name="heal">회복량</param>
    void HealthHeal(float heal);

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    void Die();
}