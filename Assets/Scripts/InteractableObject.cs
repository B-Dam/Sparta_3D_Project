using UnityEngine;

/// <summary>
/// �Ϲ� �����(�������� ��): itemData�� �ƹ��͵� �Ҵ����� ������ �ܼ� ��������� ����
/// ������ �ݱ��: itemData�� �Ҵ��ϸ�, F Ű ������ ������ ȹ��
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("�⺻ ���� ���� (�Ϲ������)")]
    public string objectName;
    [TextArea] public string description;

    [Header("������ �ݱ�� (Consumable, Equipment ��)")]
    public ItemData itemData;   // ScriptableObject �Ҵ��ϸ� �ݱ� ���
    public int pickupQuantity = 1;

    // IInteractable ����
    public string ObjectName
    {
        get
        {
            // itemData�� ������ �� �̸�, �ƴϸ� objectName
            return itemData != null ? itemData.itemName : objectName;
        }
    }

    public string Description
    {
        get
        {
            // itemData�� ������ ������ data�� description �޽���, �ƴϸ� ������ �ʱ�ȭ �� description�� ���.
            if (itemData != null)
                return $"{itemData.description}";
            return description;
        }
    }

    public void Interact()
    {
        if (itemData != null)
        {
            // ������ �ݱ�
            Inventory.Instance.AddItem(itemData, pickupQuantity);
            Debug.Log($"{itemData.itemName} x{pickupQuantity} ȹ��!");
            Destroy(gameObject);
        }
        else
        {
            // �Ϲ� ����
            Debug.Log($"����: {objectName} : {description}");
        }
    }
}
