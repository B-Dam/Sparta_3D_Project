/// <summary>
/// � ������Ʈ�� ������ & ��ȣ�ۿ롯�� �����ϵ��� ����� �������̽�
/// </summary>
public interface IInteractable
{
    string ObjectName { get; }
    string Description { get; }

    /// <summary>
    /// �÷��̾ F Ű�� ������ �� ����� ����
    /// </summary>
    void Interact();
}