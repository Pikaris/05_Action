public interface IStateMachine
{
    /// <summary>
    /// 이 상태머신의 현재
    /// </summary>
    IState State { get; }

    /// <summary>
    /// 현재 상태를 다른 상태로 전이 시키는 함수
    /// </summary>
    /// <param name="target">전이할 상태</param>
    void TransitionTo(IState target);
}