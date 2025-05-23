using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CampFire : MonoBehaviour
{
    public int damageAmount = 5;
    [Tooltip("초마다 한 번씩 데미지")]
    public float damageInterval = 1f;

    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            DealDamage(other);
        timer = damageInterval;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            DealDamage(other);
            timer = damageInterval;
        }
    }

    private void DealDamage(Collider other)
    {
        var healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null)
            healthSystem.TakeDamage(damageAmount);
    }
}
