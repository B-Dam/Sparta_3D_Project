using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject inventoryPanel;
    public Transform gridParent;
    public GameObject slotPrefab;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Button useButton;
    public Button dropButton;


    [Header("Player & Input")]
    public PlayerController playerController;
    public PlayerInput playerInput;       // 씬에 붙어 있는 PlayerInput 컴포넌트

    // InputAction을 보관할 변수
    private InputAction inventoryAction;

    private void OnEnable()
    {
        // PlayerInput에서 "Inventory" 액션을 찾아서 사용
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

    // Tab 키(Inventory 액션) 눌렀을 때
    private void OnInventoryToggle(InputAction.CallbackContext context)
    {
        bool willOpen = inventoryPanel.activeSelf == false;
        inventoryPanel.SetActive(willOpen);

        if (willOpen)
        {
            // 인벤토리 열 때
            playerController.SetCursorLocked(false);  // 커서 보이기
            playerController.CanLook = false;    // 카메라 멈추기
            RefreshUI();
        }
        else
        {
            // 인벤토리 닫을 때
            playerController.SetCursorLocked(true);  // 커서 숨기고 잠그기
            playerController.CanLook = true;    // 카메라 재개
        }
    }

    private void RefreshUI()
    {
        // 1) 기존 슬롯 삭제
        foreach (Transform t in gridParent) Destroy(t.gameObject);

        // 2) 모든 아이템에 대해 슬롯 생성
        List<Item> items = Inventory.Instance.GetAllItems();
        foreach (Item item in items)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);
            slot.SetActive(true);

            // 아이콘, 수량 세팅
            slot.transform
                .Find("Icon")
                .GetComponent<Image>()
                .sprite = item.Data.icon;

            slot.transform
                .Find("QuantityText")
                .GetComponent<TextMeshProUGUI>()
                .text = item.Quantity.ToString();

            // 슬롯 클릭 시 하단 정보 표시
            Button slotBtn = slot.AddComponent<Button>();
            int id = item.Data.id;  // 로컬 복사
            slotBtn.onClick.AddListener(() => OnSlotSelected(id));
        }
    }

    private void OnSlotSelected(int itemId)
    {
        ItemData data = Inventory.Instance.GetAllItems()
                             .First(i => i.Data.id == itemId).Data;

        // 텍스트 세팅
        itemName.text = data.itemName;
        itemDescription.text = data.description;

        // 하단 버튼 리스너 초기화
        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();

        // 사용 / 버리기 버튼 연결
        useButton.onClick.AddListener(() =>
        {
            Inventory.Instance.UseItem(itemId);
            RefreshUI();
            ClearSelectedInfo();
        });
        dropButton.onClick.AddListener(() =>
        {
            // 인벤토리에서 제거
            if (Inventory.Instance.RemoveItem(itemId, 1))
            {
                // throwPosition 위치에 월드 프리팹 생성
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
    /// 하단 선택 정보(이름,설명,버튼) 초기화
    /// </summary>
    private void ClearSelectedInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();
    }
}