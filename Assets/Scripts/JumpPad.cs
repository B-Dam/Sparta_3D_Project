using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("점프력")]
    public float jumpForce = 15f;

    private void Reset()
    {
        // Collider가 없으면 자동으로 추가해 주고, Trigger로 설정
        Collider col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 기존 위쪽 속도를 지워버리고 순간 임펄스
            Vector3 v = rb.velocity;
            v.y = 0;
            rb.velocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}