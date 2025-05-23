using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data { get; private set; }
    public int Quantity { get; private set; }

    public Item(ItemData data, int quantity = 1)
    {
        Data = data;
        Quantity = quantity;
    }

    // ���Կ��� ���� ���� �� ȣ��
    public void AddQuantity(int amount)
    {
        Quantity = Mathf.Max(0, Quantity + amount);
    }
}