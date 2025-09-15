// Unity C# — MonoBehaviour Component
// Attach to Main Camera. Smooth orbit/zoom with limits.
// Rotates ONLY while middle mouse (MMB) is held.
using UnityEngine;

public class MMBOrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                 // set by Bootstrap
    [SerializeField] float pivotHeight = 1.6f;

    [Header("Orbit (degrees)")]
    [SerializeField] float yawSpeed = 180f;   // deg/sec per mouse X
    [SerializeField] float pitchSpeed = 120f;   // deg/sec per mouse Y
    [SerializeField] float minPitch = -35f;   // look down limit
    [SerializeField] float maxPitch = 70f;    // look up limit
    [SerializeField] bool limitYaw = false;  // usually off (WoW-style turns freely)
    [SerializeField] float minYaw = -180f;
    [SerializeField] float maxYaw = 180f;

    [Header("Zoom (meters)")]
    [SerializeField] float minDistance = 2f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float zoomSpeed = 5f;    // meters per scroll unit

    [Header("Smoothing")]
    [SerializeField] float rotationSmoothTime = 0.08f;  // lower = snappier
    [SerializeField] float zoomSmoothTime = 0.12f;
    [SerializeField] float followLerp = 12f;    // position smoothing

    [Header("Collision")]
    [SerializeField] float camRadius = 0.25f;
    [SerializeField] LayerMask collisionMask = ~0;      // everything by default

    [Header("Control")]
    [SerializeField] bool rotateOnlyWhenMMB = true;

    // internal state
    float _targetYaw, _targetPitch;
    float _yaw, _pitch;                // smoothed angles
    float _yawVel, _pitchVel;          // SmoothDampAngle velocities
    float _targetDist = 5f, _dist = 5f, _distVel;
    bool _wasMMB;

    public void SetYaw(float deg) { _yaw = _targetYaw = deg; }
    public void SetPitch(float deg) { _pitch = _targetPitch = Mathf.Clamp(deg, minPitch, maxPitch); }

    void Start()
    {
        // If someone forgot to set starting distance in inspector, seed from current camera offset
        if (target)
        {
            var toCam = transform.position - (target.position + Vector3.up * pivotHeight);
            _targetDist = _dist = toCam.magnitude;
            // seed angles from current transform
            var e = transform.rotation.eulerAngles;
            _yaw = _targetYaw = e.y;
            _pitch = _targetPitch = NormalizePitch(e.x);
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // -------- Zoom (smooth) --------
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
            _targetDist = Mathf.Clamp(_targetDist - scroll * zoomSpeed, minDistance, maxDistance);

        // -------- Rotation input (MMB hold or always) --------
        bool mmb = Input.GetMouseButton(2);
        bool rotating = !rotateOnlyWhenMMB || mmb;

        // On first press, align targets to current to avoid a "jump"
        if (rotateOnlyWhenMMB && mmb && !_wasMMB)
        {
            _targetYaw = _yaw;
            _targetPitch = _pitch;
            _yawVel = _pitchVel = 0f;
        }
        _wasMMB = mmb;

        if (rotating)
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            _targetYaw += mx * yawSpeed * Time.deltaTime;
            _targetPitch -= my * pitchSpeed * Time.deltaTime;

            // limits
            _targetPitch = Mathf.Clamp(_targetPitch, minPitch, maxPitch);
            if (limitYaw)
                _targetYaw = Mathf.Clamp(_targetYaw, minYaw, maxYaw);
            else
                _targetYaw = WrapAngle(_targetYaw); // keep in -180..180 to prevent huge values
        }

        // -------- Smooth angles & distance --------
        _yaw = Mathf.SmoothDampAngle(_yaw, _targetYaw, ref _yawVel, rotationSmoothTime);
        _pitch = Mathf.SmoothDampAngle(_pitch, _targetPitch, ref _pitchVel, rotationSmoothTime);
        _dist = Mathf.SmoothDamp(_dist, _targetDist, ref _distVel, zoomSmoothTime);

        // -------- Desired camera position --------
        Vector3 pivot = target.position + Vector3.up * pivotHeight;
        Quaternion rot = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 back = rot * Vector3.back; // -forward
        Vector3 desiredPos = pivot + back * _dist;

        // -------- Collision: sphere cast from pivot to desired --------
        Vector3 toCam = desiredPos - pivot;
        float desiredLen = toCam.magnitude;
        if (desiredLen > 0.0001f)
        {
            if (Physics.SphereCast(pivot, camRadius, toCam.normalized, out var hit, desiredLen, collisionMask, QueryTriggerInteraction.Ignore))
                desiredPos = pivot + toCam.normalized * Mathf.Max(0f, hit.distance - 0.01f);
        }

        // -------- Apply smoothed position & exact rotation --------
        transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-followLerp * Time.deltaTime));
        transform.rotation = rot;
    }

    static float WrapAngle(float a)
    {
        a %= 360f; if (a > 180f) a -= 360f; if (a < -180f) a += 360f; return a;
    }
    static float NormalizePitch(float xDeg)
    {
        // Convert 0..360 euler to -180..180, then clamp later
        xDeg = WrapAngle(xDeg);
        return xDeg;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        maxPitch = Mathf.Max(minPitch + 1f, maxPitch);
        if (limitYaw) maxYaw = Mathf.Max(minYaw + 1f, maxYaw);
        minDistance = Mathf.Max(0.05f, minDistance);
        maxDistance = Mathf.Max(minDistance + 0.01f, maxDistance);
        rotationSmoothTime = Mathf.Max(0.0f, rotationSmoothTime);
        zoomSmoothTime     = Mathf.Max(0.0f, zoomSmoothTime);
        followLerp         = Mathf.Max(0.0f, followLerp);
    }
#endif
}
