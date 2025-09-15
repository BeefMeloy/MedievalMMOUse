<<<<<<< HEAD
// Unity C# — MonoBehaviour Component
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    // Read-only properties other scripts can use
    public Vector2 MoveAxis { get; private set; }   // X = A/D, Y = W/S
    public Vector2 LookDelta { get; private set; }  // X = mouse X, Y = mouse Y
    public bool SprintHeld { get; private set; }    // Left Shift

    [SerializeField] private float mouseSensitivity = 1.0f;

    private void Update()
    {
        // WASD (Unity's default axes: "Horizontal" and "Vertical")
        MoveAxis = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Mouse look (we'll use X for yaw)
        LookDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        ) * mouseSensitivity;

        // Sprint (hold)
        SprintHeld = Input.GetKey(KeyCode.LeftShift);
    }
}
=======
// Unity C# — MonoBehaviour Component
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    // Read-only properties other scripts can use
    public Vector2 MoveAxis { get; private set; }   // X = A/D, Y = W/S
    public Vector2 LookDelta { get; private set; }  // X = mouse X, Y = mouse Y
    public bool SprintHeld { get; private set; }    // Left Shift

    [SerializeField] private float mouseSensitivity = 1.0f;

    private void Update()
    {
        // WASD (Unity's default axes: "Horizontal" and "Vertical")
        MoveAxis = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Mouse look (we'll use X for yaw)
        LookDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        ) * mouseSensitivity;

        // Sprint (hold)
        SprintHeld = Input.GetKey(KeyCode.LeftShift);
    }
}
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
