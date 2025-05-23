using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ObjectInteractor : MonoBehaviour
{
    [Header("Raycast")]
    public Camera playerCamera;
    public float maxDistance = 5f;
    public LayerMask interactableLayer;

    [Header("UI Panel")]
    public GameObject infoPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI hintText;         // “[F] 상호작용”

    [Header("Input System")]
    public PlayerInput playerInput;      // PlayerInput 컴포넌트 할당
    private InputAction interactionAction;

    private IInteractable currentTarget;

    private void OnEnable()
    {
        // Interaction 액션 분기 등록
        interactionAction = playerInput.actions["Interaction"];
        interactionAction.Enable();
        interactionAction.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactionAction.performed -= OnInteract;
        interactionAction.Disable();
    }

    private void Update()
    {
        // 1) Raycast로 시선 대상 찾기
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            IInteractable target = hit.collider.GetComponent<IInteractable>();
            if (target != null)
            {
                currentTarget = target;
                ShowUI(target);
                return;
            }
        }

        // 2) 대상 없으면 UI 끄기
        currentTarget = null;
        HideUI();
    }

    // --- 여기만 public으로 노출해, PlayerInput의 Invoke Unity Events 대신 직접 콜백으로 걸어도 되고 ---
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Started || currentTarget == null) return;

        // 오직 ItemData가 있는 객체에서만 상호작용!
        InteractableObject io = currentTarget as InteractableObject;
        if (io != null && io.itemData != null)
        {
            io.Interact();
            HideUI();
            currentTarget = null;
        }
    }

    private void ShowUI(IInteractable t)
    {
        if (!infoPanel.activeSelf)
            infoPanel.SetActive(true);

        nameText.text = t.ObjectName;
        descText.text = t.Description;

        // 획득 가능 대상(itemData != null)에만 힌트 보이기
        InteractableObject io = t as InteractableObject;
        bool canPickUp = (io != null && io.itemData != null);
        hintText.enabled = canPickUp;
        if (canPickUp)
            hintText.text = "[F] 상호작용";
    }

    private void HideUI()
    {
        if (infoPanel.activeSelf)
            infoPanel.SetActive(false);
    }
}
