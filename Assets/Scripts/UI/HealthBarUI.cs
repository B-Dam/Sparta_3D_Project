using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Tooltip("�÷��̾� ������Ʈ�� ���� HealthSystem")]
    public HealthSystem healthSystem;
    [Tooltip("ü�� ������ ���ҽ�")]
    public Image barImage;

    private void OnEnable()
    {
        healthSystem.OnHealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        healthSystem.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(float normalizedHealth)
    {
        barImage.fillAmount = normalizedHealth;
    }
}
