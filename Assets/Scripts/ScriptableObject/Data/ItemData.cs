using UnityEngine;

public enum ItemType
{
    Consumable,    // �Ҹ� ������
    Equipment,     // ��� ������
    Resource       // �ڿ� ������
}

[CreateAssetMenu(menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("�ĺ��� ID")]
    public int id;

    [Header("������ �̸�")]
    public string itemName;

    [Header("������")]
    public Sprite icon;

    [Header("������ Ÿ��")]
    public ItemType itemType;

    [Header("����")]
    [TextArea] public string description;

    [Header("�Ҹ� ������ ���� : ȸ����, ���ӽð� ��")]
    public int restoreValue;
    public int effectTime;

    [Header("�Ҹ� ������ ���� : �̵��ӵ� ���� ����")]
    public float speedMultiplier = 1.5f;

    [Header("���忡 ������ Pickup Prefab")]
    public GameObject pickupPrefab;
}
