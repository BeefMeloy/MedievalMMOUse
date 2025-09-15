// Unity C# — MonoBehaviour Component (add to Player root)
using UnityEngine;

public class PlayerAnimDriver : MonoBehaviour
{
    public Animator animator;                 // Drag your Animator here (or it will auto-find)
    public string speedParam = "Speed";
    public float smoothTime = 0.10f;

    CharacterController cc;
    Rigidbody rb;
    float smoothed, velRef;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 1) Try actual velocity (CharacterController or Rigidbody)
        Vector3 v = Vector3.zero;
        if (cc) v = cc.velocity;
        else if (rb) v = rb.linearVelocity;

        // 2) If your movement doesn’t update velocity, fall back to input magnitude
        if (v.sqrMagnitude < 0.0001f)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            v = new Vector3(h, 0f, z);   // just to get a magnitude from WASD
        }

        v.y = 0f;
        float target = v.magnitude;                   // 0 = idle, 1 ≈ full input
        smoothed = Mathf.SmoothDamp(smoothed, target, ref velRef, smoothTime);

        if (animator) animator.SetFloat(speedParam, smoothed);
    }
}
