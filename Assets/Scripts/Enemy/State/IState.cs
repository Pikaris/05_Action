
/// <summary>
/// 상태가 기본적으로 가지는 인터페이스
/// </summary>
public interface IState
{
    /// <summary>
    /// 이 상태에 진입했을 때 실행되는 함수
    /// </summary>
    void Enter();

    /// <summary>
    /// 이 상태에서 나갈 때 실행되는 함수
    /// </summary>
    void Exit();

    /// <summary>
    /// 이 상태가 매 프레임마다 해야 할 일을 기록해 놓은 함수
    /// </summary>
    void Update();
}
