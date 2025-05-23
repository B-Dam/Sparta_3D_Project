using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [Header("ü�� ����")]
    public int maxHealth = 100;
    private int currentHealth;

    // ������ �Ծ��� ���� �˷��ִ� �̺�Ʈ
    public event Action<int> OnTakeDamage;
    // 0~1 ���� ��(����ȭ�� ü��)�� �ѷ��ִ� �̺�Ʈ
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        // ���� ���� �� UI�� �ʱⰪ �˷��ֱ�
        OnHealthChanged?.Invoke(1f);
    }

    /// <summary>
    /// �������� �Ծ��� �� ȣ��
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        NotifyChange();
        OnTakeDamage?.Invoke(damage);
    }

    /// <summary>
    /// ȸ�� ������ ���� ���� �� ȣ��
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        NotifyChange();
    }

    private void NotifyChange()
    {
        float normalized = currentHealth / (float)maxHealth;
        OnHealthChanged?.Invoke(normalized);
    }
}
