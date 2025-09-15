<<<<<<< HEAD
// Unity C# — MonoBehaviour Component
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.1f;
    [Header("Turning")]
    [SerializeField] private float turnSmoothTime = 0.08f;

    private CharacterController _cc;
    private PlayerInputReader _input;
    private float _turnSmoothVelocity;
    private Vector3 _velocityY;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputReader>();

        // Basic capsule setup (okay for a placeholder)
        _cc.center = new Vector3(0, 1f, 0);
        _cc.height = 2f;
        _cc.stepOffset = 0.3f;
        _cc.slopeLimit = 45f;
    }

    private void Update()
    {
        // --- Ground check & gravity ---
        bool grounded = _cc.isGrounded;
        if (grounded && _velocityY.y < 0f) _velocityY.y = -2f; // small stick-to-ground

        // --- Read input & compute move direction relative to camera yaw ---
        Vector2 move = _input.MoveAxis; // x = A/D, y = W/S
        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (Camera.main != null)
        {
            // Project camera forward/right onto horizontal plane
            Vector3 fwd = Camera.main.transform.forward; fwd.y = 0f; fwd.Normalize();
            Vector3 rgt = Camera.main.transform.right; rgt.y = 0f; rgt.Normalize();
            camForward = fwd;
            camRight = rgt;
        }

        Vector3 desiredDir = (camForward * move.y + camRight * move.x);
        if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

        // --- Rotate character toward move direction (if moving) ---
        if (desiredDir.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(desiredDir.x, desiredDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // --- Choose speed (sprint if held) ---
        float speed = _input.SprintHeld ? sprintSpeed : walkSpeed;

        // --- Horizontal move ---
        Vector3 horizontal = desiredDir * speed;

        // --- Jump (Space) ---
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _velocityY.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- Apply gravity ---
        _velocityY.y += gravity * Time.deltaTime;

        // --- Move with CharacterController (horizontal + vertical) ---
        Vector3 motion = horizontal * Time.deltaTime + _velocityY * Time.deltaTime;
        _cc.Move(motion);
    }
}
=======
// Unity C# — MonoBehaviour Component
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.1f;
    [Header("Turning")]
    [SerializeField] private float turnSmoothTime = 0.08f;

    private CharacterController _cc;
    private PlayerInputReader _input;
    private float _turnSmoothVelocity;
    private Vector3 _velocityY;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputReader>();

        // Basic capsule setup (okay for a placeholder)
        _cc.center = new Vector3(0, 1f, 0);
        _cc.height = 2f;
        _cc.stepOffset = 0.3f;
        _cc.slopeLimit = 45f;
    }

    private void Update()
    {
        // --- Ground check & gravity ---
        bool grounded = _cc.isGrounded;
        if (grounded && _velocityY.y < 0f) _velocityY.y = -2f; // small stick-to-ground

        // --- Read input & compute move direction relative to camera yaw ---
        Vector2 move = _input.MoveAxis; // x = A/D, y = W/S
        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (Camera.main != null)
        {
            // Project camera forward/right onto horizontal plane
            Vector3 fwd = Camera.main.transform.forward; fwd.y = 0f; fwd.Normalize();
            Vector3 rgt = Camera.main.transform.right; rgt.y = 0f; rgt.Normalize();
            camForward = fwd;
            camRight = rgt;
        }

        Vector3 desiredDir = (camForward * move.y + camRight * move.x);
        if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

        // --- Rotate character toward move direction (if moving) ---
        if (desiredDir.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(desiredDir.x, desiredDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // --- Choose speed (sprint if held) ---
        float speed = _input.SprintHeld ? sprintSpeed : walkSpeed;

        // --- Horizontal move ---
        Vector3 horizontal = desiredDir * speed;

        // --- Jump (Space) ---
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _velocityY.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- Apply gravity ---
        _velocityY.y += gravity * Time.deltaTime;

        // --- Move with CharacterController (horizontal + vertical) ---
        Vector3 motion = horizontal * Time.deltaTime + _velocityY * Time.deltaTime;
        _cc.Move(motion);
    }
}
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
