<<<<<<< HEAD
using UnityEngine;

[AddComponentMenu("Gameplay/Character Info")]
public class CharacterInfo : MonoBehaviour
{
    [Tooltip("Shown on the player frame. If empty, uses this GameObject's name.")]
    public string displayName = "";

    [Tooltip("Custom head PNG imported as Sprite (2D and UI).")]
    public Sprite portrait;

    [Tooltip("Auto-found if left empty.")]
    public Health health;

    public string DisplayName => string.IsNullOrEmpty(displayName) ? gameObject.name : displayName;
    public float HealthNormalized => health ? health.Normalized : 0f;

    private void Awake()
    {
        if (!health) health = GetComponent<Health>() ?? GetComponentInParent<Health>();
        if (string.IsNullOrEmpty(displayName)) displayName = gameObject.name;
    }
}
=======
using UnityEngine;

[AddComponentMenu("Gameplay/Character Info")]
public class CharacterInfo : MonoBehaviour
{
    [Tooltip("Shown on the player frame. If empty, uses this GameObject's name.")]
    public string displayName = "";

    [Tooltip("Custom head PNG imported as Sprite (2D and UI).")]
    public Sprite portrait;

    [Tooltip("Auto-found if left empty.")]
    public Health health;

    public string DisplayName => string.IsNullOrEmpty(displayName) ? gameObject.name : displayName;
    public float HealthNormalized => health ? health.Normalized : 0f;

    private void Awake()
    {
        if (!health) health = GetComponent<Health>() ?? GetComponentInParent<Health>();
        if (string.IsNullOrEmpty(displayName)) displayName = gameObject.name;
    }
}
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
