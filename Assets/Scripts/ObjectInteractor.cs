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
    public TextMeshProUGUI hintText;         // ��[F] ��ȣ�ۿ롱

    [Header("Input System")]
    public PlayerInput playerInput;      // PlayerInput ������Ʈ �Ҵ�
    private InputAction interactionAction;

    private IInteractable currentTarget;

    private void OnEnable()
    {
        // Interaction �׼� �б� ���
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
        // 1) Raycast�� �ü� ��� ã��
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

        // 2) ��� ������ UI ����
        currentTarget = null;
        HideUI();
    }

    // --- ���⸸ public���� ������, PlayerInput�� Invoke Unity Events ��� ���� �ݹ����� �ɾ �ǰ� ---
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Started || currentTarget == null) return;

        // ���� ItemData�� �ִ� ��ü������ ��ȣ�ۿ�!
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

        // ȹ�� ���� ���(itemData != null)���� ��Ʈ ���̱�
        InteractableObject io = t as InteractableObject;
        bool canPickUp = (io != null && io.itemData != null);
        hintText.enabled = canPickUp;
        if (canPickUp)
            hintText.text = "[F] ��ȣ�ۿ�";
    }

    private void HideUI()
    {
        if (infoPanel.activeSelf)
            infoPanel.SetActive(false);
    }
}
