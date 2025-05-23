using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    // 실제 보유 중인 아이템(ID → Item)
    private Dictionary<int, Item> items = new Dictionary<int, Item>();

    // 인벤토리 내용이 바뀔 때마다 UI에 알리기 위한 이벤트
    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    /// <summary>아이템 획득</summary>
    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (items.ContainsKey(itemData.id))
            items[itemData.id].AddQuantity(amount);
        else
            items[itemData.id] = new Item(itemData, amount);

        OnInventoryChanged?.Invoke();
    }

    /// <summary>소모형 아이템 사용 (회복/버프)</summary>
    public bool UseItem(int itemId)
    {
        if (!items.ContainsKey(itemId) || items[itemId].Quantity <= 0)
            return false;

        var item = items[itemId];
        if (item.Data.itemType != ItemType.Consumable)
            return false;

        // 체력 회복
        var hs = FindObjectOfType<HealthSystem>();
        if (hs != null)
            hs.Heal(item.Data.restoreValue);

        // 속도 버프
        var pc = FindObjectOfType<PlayerController>();
        if (pc != null)
            pc.ApplySpeedBuff(item.Data.speedMultiplier, item.Data.effectTime);

        // 개수 차감
        item.AddQuantity(-1);
        if (item.Quantity == 0) items.Remove(itemId);

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>아이템 버리기 (월드에 프리팹 생성 전제)</summary>
    public bool RemoveItem(int itemId, int amount = 1)
    {
        if (!items.ContainsKey(itemId) || items[itemId].Quantity < amount)
            return false;

        items[itemId].AddQuantity(-amount);
        if (items[itemId].Quantity == 0)
            items.Remove(itemId);

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>현재 보유한 모든 Item 리스트 조회</summary>
    public List<Item> GetAllItems() => items.Values.ToList();
}