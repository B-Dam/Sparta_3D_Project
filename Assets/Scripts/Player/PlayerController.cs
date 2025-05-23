using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f;

    [Header("���� ����")]
    public float jumpPower = 5f;
    public LayerMask groundLayerMask;

    [Header("ī�޶� ȸ��")]
    [Tooltip("ī�޶� �پ� �ִ� ������Ʈ")]
    public Transform cameraContainer;
    [Tooltip("���� ȸ�� �ּ�/�ִ�")]
    public float minXLook = -60f;
    public float maxXLook = 60f;
    [Tooltip("���콺 ����")]
    public float lookSensitivity = 1.5f;

    [Header("������ ���� ��ġ")]
    public Transform throwPosition;


    // �ܺο� �κ��丮 ��� �̺�Ʈ ����
    public Action onInventoryToggle;

    // ���� ����
    private Rigidbody _rigidbody;
    private Vector2 curMovementInput;
    private Vector2 mouseDelta;
    private float camCurXRot;
    private bool canLook = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ���� ������ �� Ŀ�� ��ױ�
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    private void Move()
    {
        // ���� X,Z �������� �̵�
        Vector3 dir = transform.forward * curMovementInput.y
                    + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        // �߷�(�Ǵ� ���� �ӵ�)�� �״�� ����
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        // ����
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = Vector3.right * -camCurXRot;
        // ����
        transform.Rotate(Vector3.up * mouseDelta.x * lookSensitivity, Space.World);
    }

    private bool IsGrounded()
    {
        // 4�������� ª�� ����ĳ��Ʈ ���� �ٴ� ����
        Vector3 origin = transform.position + Vector3.up * 0.01f;
        Vector3[] dirs = {
            transform.forward * 0.2f,
            -transform.forward * 0.2f,
            transform.right   * 0.2f,
            -transform.right   * 0.2f
        };
        foreach (Vector3 off in dirs)
        {
            if (Physics.Raycast(origin + off, Vector3.down, 0.1f, groundLayerMask))
                return true;
        }
        return false;
    }
    public bool CanLook
    {
        get { return canLook; }
        set { canLook = value; }
    }

    public void SetCursorLocked(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // ���� �ڷ�ƾ ����
    private Coroutine speedBuffCoroutine;

    /// <summary>
    /// �̵��ӵ��� multiplier ���, duration �ʰ� �����մϴ�.
    /// </summary>
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        // �̹� ���� ���̸� �ʱ�ȭ
        if (speedBuffCoroutine != null)
            StopCoroutine(speedBuffCoroutine);

        speedBuffCoroutine = StartCoroutine(SpeedBuffRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBuffRoutine(float multiplier, float duration)
    {
        float originalSpeed = moveSpeed;
        moveSpeed = originalSpeed * multiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        speedBuffCoroutine = null;
    }


    // On �޼������ PlayerInput > Invoke Unity Events�� ���ε��ϱ�

    public void OnMove(InputAction.CallbackContext context)
    {
        curMovementInput = context.phase == InputActionPhase.Performed
                         ? context.ReadValue<Vector2>()
                         : Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.phase == InputActionPhase.Performed
                   ? context.ReadValue<Vector2>()
                   : Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            onInventoryToggle?.Invoke();
            // Ŀ�� ���
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            canLook = locked;
        }
    }
}
