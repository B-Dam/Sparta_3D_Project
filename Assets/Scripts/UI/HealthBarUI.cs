using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Tooltip("플레이어 오브젝트에 붙은 HealthSystem")]
    public HealthSystem healthSystem;
    [Tooltip("체력 게이지 리소스")]
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
