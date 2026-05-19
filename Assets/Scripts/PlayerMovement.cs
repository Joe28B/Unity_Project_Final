using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

/// <summary>
/// Isometric movement + sprint for XR Origin used as a non-VR player.
/// Handles gravity properly so the player never flies or falls through floors.
///
/// SETUP on XR Origin GameObject:
///   1. Add CharacterController (Height 1.8, Center Y 0.9, Skin Width 0.08)
///   2. Add this script
///   3. Add PlayerInput (Send Messages, assign InputSystem_Actions asset)
///      - "Move"   action: Value / Vector2
///      - "Sprint" action: Button (e.g. Left Shift)
///   4. Set Ground Mask to your floor/terrain layers
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float walkSpeed   = 5f;
    public float sprintSpeed = 10f;
    public float rotationSpeed = 12f;

    [Header("Gravity / Ground")]
    public float gravity = -20f;
    [Tooltip("Which layers count as ground. Set this to your floor layer(s).")]
    public LayerMask groundMask = ~8;

    // ── private ──────────────────────────────────────────────────
    private CharacterController _cc;
    private Vector2 _moveInput;
    private bool    _isSprinting;
    private float   _verticalVelocity;
    private bool    _isGrounded;
    private Transform _cam;

    // ── Input (PlayerInput Send Messages) ────────────────────────
    void OnMove(InputValue v)   => _moveInput   = v.Get<Vector2>();
    void OnSprint(InputValue v) => _isSprinting = v.isPressed;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (Camera.main != null)
            _cam = Camera.main.transform;
        else
            Debug.LogWarning("PlayerMovement: No MainCamera found. " +
                             "Tag the XR camera as 'MainCamera'.");
    }

    void Update()
    {
        CheckGround();
        Move();
        ApplyGravity();
    }

    void CheckGround()
    {
        // Sphere cast just beneath the CharacterController feet
        float checkRadius = _cc.radius + 0.05f;
        Vector3 bottom = transform.position +
                         Vector3.down * (_cc.height * 0.5f - _cc.radius + 0.02f);

        _isGrounded = Physics.CheckSphere(bottom, checkRadius,
                                          groundMask, QueryTriggerInteraction.Ignore);
        if (_isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f; // small constant to keep grounded
    }

    void Move()
    {
        if (_moveInput.sqrMagnitude < 0.01f) return;

        float speed = _isSprinting ? sprintSpeed : walkSpeed;

        // Flatten camera axes — isometric style, no vertical component
        Vector3 camFwd = _cam != null
            ? Vector3.ProjectOnPlane(_cam.forward, Vector3.up).normalized
            : Vector3.forward;
        Vector3 camRight = _cam != null
            ? Vector3.ProjectOnPlane(_cam.right, Vector3.up).normalized
            : Vector3.right;

        Vector3 dir = (camFwd * _moveInput.y + camRight * _moveInput.x).normalized;
        _cc.Move(dir * speed * Time.deltaTime);

        // Rotate XR Origin to face movement direction
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                                               rotationSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        _verticalVelocity += gravity * Time.deltaTime;
        // Cap fall speed so player doesn't clip through thin floors at high speeds
        _verticalVelocity = Mathf.Max(_verticalVelocity, -30f);
        _cc.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
    }
}
