using UnityEngine;

public enum ItemType
{
    Consumable,    // 소모 아이템
    Equipment,     // 장비 아이템
    Resource       // 자원 아이템
}

[CreateAssetMenu(menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("식별용 ID")]
    public int id;

    [Header("아이템 이름")]
    public string itemName;

    [Header("아이콘")]
    public Sprite icon;

    [Header("아이템 타입")]
    public ItemType itemType;

    [Header("설명")]
    [TextArea] public string description;

    [Header("소모 아이템 전용 : 회복량, 지속시간 등")]
    public int restoreValue;
    public int effectTime;

    [Header("소모 아이템 전용 : 이동속도 버프 배율")]
    public float speedMultiplier = 1.5f;

    [Header("월드에 생성할 Pickup Prefab")]
    public GameObject pickupPrefab;
}
