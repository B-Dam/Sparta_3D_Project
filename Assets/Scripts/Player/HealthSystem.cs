using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 100;
    private int currentHealth;

    // 데미지 입었을 때만 알려주는 이벤트
    public event Action<int> OnTakeDamage;
    // 0~1 사이 값(정규화된 체력)을 뿌려주는 이벤트
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        // 게임 시작 시 UI에 초기값 알려주기
        OnHealthChanged?.Invoke(1f);
    }

    /// <summary>
    /// 데미지를 입었을 때 호출
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        NotifyChange();
        OnTakeDamage?.Invoke(damage);
    }

    /// <summary>
    /// 회복 아이템 등을 썼을 때 호출
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
