using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("������")]
    public float jumpForce = 15f;

    private void Reset()
    {
        // Collider�� ������ �ڵ����� �߰��� �ְ�, Trigger�� ����
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
            // ���� ���� �ӵ��� ���������� ���� ���޽�
            Vector3 v = rb.velocity;
            v.y = 0;
            rb.velocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}