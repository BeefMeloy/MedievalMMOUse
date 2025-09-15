using UnityEngine;

public enum EntityCategory
{
    Friendly,
    Hostile,
    Neutral,
    Player,
    QuestGiver,
    Vendor,
    Boss,
    Elite,
    Critter
}

[CreateAssetMenu(fileName = "New Entity Type", menuName = "Game/Entity Type")]
public class EnemyType : ScriptableObject
{
    [Header("Basic Info")]
    [Tooltip("Leave empty to use GameObject name automatically")]
    public string entityNameOverride = "";  // Changed from entityName
    public Sprite portrait;
    public EntityCategory category = EntityCategory.Hostile;
    public int defaultLevel = 1;

    [Header("UI Display")]
    public Color nameColor = Color.white;
    public Color frameColor = Color.red;
    public bool showLevel = true;
    public bool showHealthBar = true;
    public string titlePrefix = "";

    [Header("Name Processing")]
    [Tooltip("Remove common prefab suffixes like (Clone), _01, etc.")]
    public bool cleanPrefabNames = true;
    [Tooltip("Convert CamelCase/PascalCase to spaced words")]
    public bool addSpacesToCamelCase = true;

    [Header("Circle Appearance")]
    public Color primaryColor = Color.red;
    public Color secondaryColor = Color.yellow;
    public float circleRadius = 1.5f;
    public float glowIntensity = 2f;

    [Header("Animation")]
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.3f;
    public float rotationSpeed = 1f;

    [Header("Advanced Effects")]
    public float innerRadius = 0.6f;
    public float outerRadius = 0.8f;
    public float edgeSoftness = 0.1f;
    public float outerGlow = 1f;
    public float noiseScale = 5f;
    public float noiseSpeed = 1f;

    [Header("Audio")]
    public AudioClip targetSound;
    public AudioClip hoverSound;

    // Dynamic name processing
    public string GetEntityName(string gameObjectName)
    {
        // Use override if provided
        if (!string.IsNullOrEmpty(entityNameOverride))
            return entityNameOverride;

        string processedName = gameObjectName;

        if (cleanPrefabNames)
        {
            processedName = CleanPrefabName(processedName);
        }

        if (addSpacesToCamelCase)
        {
            processedName = AddSpacesToCamelCase(processedName);
        }

        return processedName;
    }

    // Helper methods for UI
    public string GetFormattedName(string gameObjectName, int level = -1)
    {
        string baseName = GetEntityName(gameObjectName);
        string result = "";

        if (!string.IsNullOrEmpty(titlePrefix))
            result += $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor)}>{titlePrefix} </color>";

        result += $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor)}>{baseName}</color>";

        if (showLevel && level > 0)
            result += $" <size=80%><color=yellow>[{level}]</color></size>";

        return result;
    }

    private string CleanPrefabName(string name)
    {
        // Remove common prefab suffixes
        string[] suffixesToRemove = { "(Clone)", "_Prefab", "_prefab", "_01", "_02", "_03", "_04", "_05" };

        foreach (string suffix in suffixesToRemove)
        {
            if (name.EndsWith(suffix))
            {
                name = name.Substring(0, name.Length - suffix.Length);
            }
        }

        // Remove numbers at the end (like Goblin001 -> Goblin)
        while (name.Length > 0 && char.IsDigit(name[name.Length - 1]))
        {
            name = name.Substring(0, name.Length - 1);
        }

        // Remove underscores at the end
        name = name.TrimEnd('_');

        return name;
    }

    private string AddSpacesToCamelCase(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        System.Text.StringBuilder result = new System.Text.StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            char current = text[i];

            // Add space before uppercase letters (except first character)
            if (i > 0 && char.IsUpper(current) && char.IsLower(text[i - 1]))
            {
                result.Append(' ');
            }

            result.Append(current);
        }

        return result.ToString();
    }

    public bool IsFriendly() => category == EntityCategory.Friendly || category == EntityCategory.QuestGiver || category == EntityCategory.Vendor;
    public bool IsHostile() => category == EntityCategory.Hostile || category == EntityCategory.Boss || category == EntityCategory.Elite;
    public bool IsInteractable() => category == EntityCategory.QuestGiver || category == EntityCategory.Vendor;
}