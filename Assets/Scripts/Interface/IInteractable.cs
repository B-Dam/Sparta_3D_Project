/// <summary>
/// 어떤 오브젝트든 ‘조사 & 상호작용’이 가능하도록 만드는 인터페이스
/// </summary>
public interface IInteractable
{
    string ObjectName { get; }
    string Description { get; }

    /// <summary>
    /// 플레이어가 F 키를 눌렀을 때 실행될 동작
    /// </summary>
    void Interact();
}