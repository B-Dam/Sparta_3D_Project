using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject inventoryPanel;
    public Transform gridParent;
    public GameObject slotPrefab;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Button useButton;
    public Button dropButton;


    [Header("Player & Input")]
    public PlayerController playerController;
    public PlayerInput playerInput;       // ���� �پ� �ִ� PlayerInput ������Ʈ

    // InputAction�� ������ ����
    private InputAction inventoryAction;

    private void OnEnable()
    {
        // PlayerInput���� "Inventory" �׼��� ã�Ƽ� ���
        inventoryAction = playerInput.actions["Inventory"];
        inventoryAction.Enable();
        inventoryAction.performed += OnInventoryToggle;

        Inventory.Instance.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChanged -= RefreshUI;

        inventoryAction.performed -= OnInventoryToggle;
        inventoryAction.Disable();
    }

    // Tab Ű(Inventory �׼�) ������ ��
    private void OnInventoryToggle(InputAction.CallbackContext context)
    {
        bool willOpen = inventoryPanel.activeSelf == false;
        inventoryPanel.SetActive(willOpen);

        if (willOpen)
        {
            // �κ��丮 �� ��
            playerController.SetCursorLocked(false);  // Ŀ�� ���̱�
            playerController.CanLook = false;    // ī�޶� ���߱�
            RefreshUI();
        }
        else
        {
            // �κ��丮 ���� ��
            playerController.SetCursorLocked(true);  // Ŀ�� ����� ��ױ�
            playerController.CanLook = true;    // ī�޶� �簳
        }
    }

    private void RefreshUI()
    {
        // 1) ���� ���� ����
        foreach (Transform t in gridParent) Destroy(t.gameObject);

        // 2) ��� �����ۿ� ���� ���� ����
        List<Item> items = Inventory.Instance.GetAllItems();
        foreach (Item item in items)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);
            slot.SetActive(true);

            // ������, ���� ����
            slot.transform
                .Find("Icon")
                .GetComponent<Image>()
                .sprite = item.Data.icon;

            slot.transform
                .Find("QuantityText")
                .GetComponent<TextMeshProUGUI>()
                .text = item.Quantity.ToString();

            // ���� Ŭ�� �� �ϴ� ���� ǥ��
            Button slotBtn = slot.AddComponent<Button>();
            int id = item.Data.id;  // ���� ����
            slotBtn.onClick.AddListener(() => OnSlotSelected(id));
        }
    }

    private void OnSlotSelected(int itemId)
    {
        ItemData data = Inventory.Instance.GetAllItems()
                             .First(i => i.Data.id == itemId).Data;

        // �ؽ�Ʈ ����
        itemName.text = data.itemName;
        itemDescription.text = data.description;

        // �ϴ� ��ư ������ �ʱ�ȭ
        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();

        // ��� / ������ ��ư ����
        useButton.onClick.AddListener(() =>
        {
            Inventory.Instance.UseItem(itemId);
            RefreshUI();
            ClearSelectedInfo();
        });
        dropButton.onClick.AddListener(() =>
        {
            // �κ��丮���� ����
            if (Inventory.Instance.RemoveItem(itemId, 1))
            {
                // throwPosition ��ġ�� ���� ������ ����
                Instantiate(
                    data.pickupPrefab,
                    playerController.throwPosition.position,
                    Quaternion.identity
                );
            }
            RefreshUI();
            ClearSelectedInfo();
        });
    }

    /// <summary>
    /// �ϴ� ���� ����(�̸�,����,��ư) �ʱ�ȭ
    /// </summary>
    private void ClearSelectedInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();
    }
}