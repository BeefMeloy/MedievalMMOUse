using UnityEngine;

[AddComponentMenu("Gameplay/Targetable")]
public class Targetable : MonoBehaviour
{
    [Header("Basic Info")]
    [Tooltip("Override name. If empty, uses GameObject name processed by EntityType")]
    public string displayNameOverride = "";

    [Tooltip("Headshot/portrait sprite (imported as Sprite (2D and UI)).")]
    public Sprite portrait;

    [Tooltip("Optional; auto-found if left empty.")]
    public Health health;

    [Header("Entity Type")]
    [Tooltip("Defines appearance, behavior, and UI treatment")]
    public EnemyType entityType;
    public int level = 1;

    [Header("Additional Info")]
    public string guildName = "";
    public Sprite customPortrait; // Overrides entityType.portrait if set

    // Enhanced properties with dynamic naming
    public string DisplayName
    {
        get
        {
            if (!string.IsNullOrEmpty(displayNameOverride))
                return displayNameOverride;

            if (entityType)
                return entityType.GetEntityName(gameObject.name);

            return gameObject.name;
        }
    }

    public float HealthNormalized => health ? health.Normalized : 0f;
    public Sprite Portrait => customPortrait ? customPortrait : (entityType ? entityType.portrait : portrait);
    public string FormattedName => entityType ? entityType.GetFormattedName(gameObject.name, level) : DisplayName;
    public Color NameColor => entityType ? entityType.nameColor : Color.white;
    public Color FrameColor => entityType ? entityType.frameColor : Color.gray;
    public bool IsInteractable => entityType && entityType.IsInteractable();

    private void Awake()
    {
        if (!health) health = GetComponentInParent<Health>() ?? GetComponent<Health>();

        // Auto-assign level if using entity type
        if (entityType && level <= 1) level = entityType.defaultLevel;
    }

    public string GetFullDisplayText()
    {
        string result = FormattedName;

        if (!string.IsNullOrEmpty(guildName))
            result += $"\n<size=70%><{guildName}></size>";

        return result;
    }
}