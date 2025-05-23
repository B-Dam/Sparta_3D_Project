using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    // ���� ���� ���� ������(ID �� Item)
    private Dictionary<int, Item> items = new Dictionary<int, Item>();

    // �κ��丮 ������ �ٲ� ������ UI�� �˸��� ���� �̺�Ʈ
    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    /// <summary>������ ȹ��</summary>
    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (items.ContainsKey(itemData.id))
            items[itemData.id].AddQuantity(amount);
        else
            items[itemData.id] = new Item(itemData, amount);

        OnInventoryChanged?.Invoke();
    }

    /// <summary>�Ҹ��� ������ ��� (ȸ��/����)</summary>
    public bool UseItem(int itemId)
    {
        if (!items.ContainsKey(itemId) || items[itemId].Quantity <= 0)
            return false;

        var item = items[itemId];
        if (item.Data.itemType != ItemType.Consumable)
            return false;

        // ü�� ȸ��
        var hs = FindObjectOfType<HealthSystem>();
        if (hs != null)
            hs.Heal(item.Data.restoreValue);

        // �ӵ� ����
        var pc = FindObjectOfType<PlayerController>();
        if (pc != null)
            pc.ApplySpeedBuff(item.Data.speedMultiplier, item.Data.effectTime);

        // ���� ����
        item.AddQuantity(-1);
        if (item.Quantity == 0) items.Remove(itemId);

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>������ ������ (���忡 ������ ���� ����)</summary>
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

    /// <summary>���� ������ ��� Item ����Ʈ ��ȸ</summary>
    public List<Item> GetAllItems() => items.Values.ToList();
}