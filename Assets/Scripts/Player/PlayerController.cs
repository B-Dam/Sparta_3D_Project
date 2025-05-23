using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 관련")]
    public float moveSpeed = 5f;

    [Header("점프 관련")]
    public float jumpPower = 5f;
    public LayerMask groundLayerMask;

    [Header("카메라 회전")]
    [Tooltip("카메라가 붙어 있는 오브젝트")]
    public Transform cameraContainer;
    [Tooltip("수직 회전 최소/최대")]
    public float minXLook = -60f;
    public float maxXLook = 60f;
    [Tooltip("마우스 감도")]
    public float lookSensitivity = 1.5f;

    [Header("아이템 버릴 위치")]
    public Transform throwPosition;


    // 외부에 인벤토리 토글 이벤트 제공
    public Action onInventoryToggle;

    // 내부 상태
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
        // 게임 시작할 때 커서 잠그기
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
        // 월드 X,Z 방향으로 이동
        Vector3 dir = transform.forward * curMovementInput.y
                    + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        // 중력(또는 낙하 속도)은 그대로 유지
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        // 수직
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = Vector3.right * -camCurXRot;
        // 수평
        transform.Rotate(Vector3.up * mouseDelta.x * lookSensitivity, Space.World);
    }

    private bool IsGrounded()
    {
        // 4방향으로 짧은 레이캐스트 쏴서 바닥 감지
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

    // 버프 코루틴 참조
    private Coroutine speedBuffCoroutine;

    /// <summary>
    /// 이동속도를 multiplier 배로, duration 초간 유지합니다.
    /// </summary>
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        // 이미 버프 중이면 초기화
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


    // On 메서드들은 PlayerInput > Invoke Unity Events로 바인딩하기

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
            // 커서 토글
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            canLook = locked;
        }
    }
}
