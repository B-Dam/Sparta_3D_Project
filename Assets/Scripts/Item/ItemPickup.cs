using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("�ֿ� ������ ������")]
    public ItemData itemData;     // ScriptableObject
    public int quantity = 1;      // �� �� �ֿ���

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾� �±� Ȯ��
        if (!other.CompareTag("Player")) return;

        // Inventory�� �߰�
        Inventory.Instance.AddItem(itemData, quantity);

        // ������ ��Ȱ��ȭ �Ǵ� �ı�
        Destroy(gameObject);
    }
}