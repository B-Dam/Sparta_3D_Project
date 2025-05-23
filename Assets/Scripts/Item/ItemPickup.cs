using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("주울 아이템 데이터")]
    public ItemData itemData;     // ScriptableObject
    public int quantity = 1;      // 몇 개 주울지

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그 확인
        if (!other.CompareTag("Player")) return;

        // Inventory에 추가
        Inventory.Instance.AddItem(itemData, quantity);

        // 프리팹 비활성화 또는 파괴
        Destroy(gameObject);
    }
}