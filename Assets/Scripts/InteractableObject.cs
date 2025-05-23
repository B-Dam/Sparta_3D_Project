using UnityEngine;

/// <summary>
/// 일반 조사용(문·상자 등): itemData에 아무것도 할당하지 않으면 단순 조사용으로 동작
/// 아이템 줍기용: itemData를 할당하면, F 키 누르면 아이템 획득
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("기본 조사 정보 (일반조사용)")]
    public string objectName;
    [TextArea] public string description;

    [Header("아이템 줍기용 (Consumable, Equipment 등)")]
    public ItemData itemData;   // ScriptableObject 할당하면 줍기 대상
    public int pickupQuantity = 1;

    // IInteractable 구현
    public string ObjectName
    {
        get
        {
            // itemData가 있으면 그 이름, 아니면 objectName
            return itemData != null ? itemData.itemName : objectName;
        }
    }

    public string Description
    {
        get
        {
            // itemData가 있으면 아이템 data의 description 메시지, 아니면 위에서 초기화 한 description을 출력.
            if (itemData != null)
                return $"{itemData.description}";
            return description;
        }
    }

    public void Interact()
    {
        if (itemData != null)
        {
            // 아이템 줍기
            Inventory.Instance.AddItem(itemData, pickupQuantity);
            Debug.Log($"{itemData.itemName} x{pickupQuantity} 획득!");
            Destroy(gameObject);
        }
        else
        {
            // 일반 조사
            Debug.Log($"조사: {objectName} : {description}");
        }
    }
}
