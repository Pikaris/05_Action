/// <summary>
/// 돈을 소유할 수 있음을 나타내는 인터페이스
/// </summary>
public interface IMoneyContainer
{
    /// <summary>
    /// 돈에 접근하기 위한 프로퍼티
    /// </summary>
    public int Money { get; set; }
}