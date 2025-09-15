using UnityEngine;

[AddComponentMenu("Gameplay/Highlightable (Advanced)")]
public class Highlightable : MonoBehaviour
{
    [Header("Material Highlighting")]
    [SerializeField] private Color highlightColor = Color.yellow;

    [Header("Floor Circle")]
    [SerializeField] private bool useFloorCircle = true;
    [SerializeField] private Material advancedCircleMaterial;
    [SerializeField] private EnemyType enemyType;

    [Header("Manual Override (if no EnemyType)")]
    [SerializeField] private Color primaryColor = Color.red;
    [SerializeField] private Color secondaryColor = Color.yellow;
    [SerializeField] private float circleRadius = 1.5f;
    [SerializeField] private float glowIntensity = 2f;
    [SerializeField] private float pulseSpeed = 2f;

    private Renderer[] rends;
    private int[][] colorPropIds;
    private Color[][] original;
    private GameObject floorCircle;
    private Renderer circleRenderer;
    private Material circleMaterialInstance;
    private MaterialPropertyBlock propertyBlock;

    public bool IsHighlighted { get; private set; }

    // Shader property IDs for performance
    static readonly int ColorID = Shader.PropertyToID("_Color");
    static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    static readonly int GlowColorID = Shader.PropertyToID("_GlowColor");
    static readonly int InnerGlowColorID = Shader.PropertyToID("_InnerGlowColor");
    static readonly int GlowIntensityID = Shader.PropertyToID("_GlowIntensity");
    static readonly int PulseSpeedID = Shader.PropertyToID("_PulseSpeed");
    static readonly int PulseIntensityID = Shader.PropertyToID("_PulseIntensity");
    static readonly int RotationSpeedID = Shader.PropertyToID("_RotationSpeed");
    static readonly int InnerRadiusID = Shader.PropertyToID("_InnerRadius");
    static readonly int OuterRadiusID = Shader.PropertyToID("_OuterRadius");
    static readonly int EdgeSoftnessID = Shader.PropertyToID("_EdgeSoftness");
    static readonly int OuterGlowID = Shader.PropertyToID("_OuterGlow");
    static readonly int NoiseScaleID = Shader.PropertyToID("_NoiseScale");
    static readonly int NoiseSpeedID = Shader.PropertyToID("_NoiseSpeed");

    const string EmissionKW = "_EMISSION";

    private void Awake()
    {
        SetupMaterialHighlighting();
        if (useFloorCircle) SetupAdvancedFloorCircle();
    }

    private void SetupMaterialHighlighting()
    {
        rends = GetComponentsInChildren<Renderer>(true);
        colorPropIds = new int[rends.Length][];
        original = new Color[rends.Length][];

        for (int r = 0; r < rends.Length; r++)
        {
            var mats = rends[r].materials;
            colorPropIds[r] = new int[mats.Length];
            original[r] = new Color[mats.Length];

            for (int m = 0; m < mats.Length; m++)
            {
                var mat = mats[m];
                int id = -1;

                if (mat.HasProperty(ColorID)) id = ColorID;
                else if (mat.HasProperty(BaseColorID)) id = BaseColorID;
                else if (mat.HasProperty(EmissionColorID)) { id = EmissionColorID; mat.EnableKeyword(EmissionKW); }

                colorPropIds[r][m] = id;
                original[r][m] = (id >= 0) ? mat.GetColor(id) : Color.clear;
            }
        }
    }

    private void SetupAdvancedFloorCircle()
    {
        // Create circle GameObject
        floorCircle = GameObject.CreatePrimitive(PrimitiveType.Quad);
        floorCircle.name = "AdvancedTargetCircle";
        floorCircle.transform.SetParent(transform);

        // Get radius from enemy type or use manual override
        float radius = enemyType ? enemyType.circleRadius : circleRadius;

        // Position it slightly above ground
        floorCircle.transform.localPosition = new Vector3(0, 0.01f, 0);
        floorCircle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        floorCircle.transform.localScale = Vector3.one * radius;

        // Remove collider
        if (floorCircle.GetComponent<Collider>())
            DestroyImmediate(floorCircle.GetComponent<Collider>());

        // Setup advanced material
        circleRenderer = floorCircle.GetComponent<Renderer>();

        if (advancedCircleMaterial)
        {
            // Create instance to avoid affecting other enemies
            circleMaterialInstance = new Material(advancedCircleMaterial);
            circleRenderer.material = circleMaterialInstance;

            // Configure shader properties based on enemy type
            ConfigureShaderProperties();
        }
        else
        {
            Debug.LogWarning($"No advanced circle material assigned to {gameObject.name}. Using fallback.");
            CreateFallbackMaterial();
        }

        propertyBlock = new MaterialPropertyBlock();
        floorCircle.SetActive(false);
    }

    private void ConfigureShaderProperties()
    {
        if (!circleMaterialInstance) return;

        if (enemyType)
        {
            // Use enemy type settings
            circleMaterialInstance.SetColor(GlowColorID, enemyType.primaryColor);
            circleMaterialInstance.SetColor(InnerGlowColorID, enemyType.secondaryColor);
            circleMaterialInstance.SetFloat(GlowIntensityID, enemyType.glowIntensity);
            circleMaterialInstance.SetFloat(PulseSpeedID, enemyType.pulseSpeed);
            circleMaterialInstance.SetFloat(PulseIntensityID, enemyType.pulseIntensity);
            circleMaterialInstance.SetFloat(RotationSpeedID, enemyType.rotationSpeed);
            circleMaterialInstance.SetFloat(InnerRadiusID, enemyType.innerRadius);
            circleMaterialInstance.SetFloat(OuterRadiusID, enemyType.outerRadius);
            circleMaterialInstance.SetFloat(EdgeSoftnessID, enemyType.edgeSoftness);
            circleMaterialInstance.SetFloat(OuterGlowID, enemyType.outerGlow);
            circleMaterialInstance.SetFloat(NoiseScaleID, enemyType.noiseScale);
            circleMaterialInstance.SetFloat(NoiseSpeedID, enemyType.noiseSpeed);
        }
        else
        {
            // Use manual override settings
            circleMaterialInstance.SetColor(GlowColorID, primaryColor);
            circleMaterialInstance.SetColor(InnerGlowColorID, secondaryColor);
            circleMaterialInstance.SetFloat(GlowIntensityID, glowIntensity);
            circleMaterialInstance.SetFloat(PulseSpeedID, pulseSpeed);
            circleMaterialInstance.SetFloat(PulseIntensityID, 0.3f);
            circleMaterialInstance.SetFloat(RotationSpeedID, 1f);
        }
    }

    private void CreateFallbackMaterial()
    {
        circleMaterialInstance = new Material(Shader.Find("Sprites/Default"));
        Color color = enemyType ? enemyType.primaryColor : primaryColor;
        circleMaterialInstance.color = new Color(color.r, color.g, color.b, 0.7f);
        circleRenderer.material = circleMaterialInstance;
    }

    public void SetHighlighted(bool on)
    {
        if (IsHighlighted == on) return;
        IsHighlighted = on;

        // Handle material highlighting (your original code)
        for (int r = 0; r < rends.Length; r++)
        {
            var mats = rends[r].materials;
            for (int m = 0; m < mats.Length; m++)
            {
                int id = colorPropIds[r][m];
                if (id < 0) continue;
                var mat = mats[m];

                if (id == EmissionColorID && !on) mat.DisableKeyword(EmissionKW);
                if (id == EmissionColorID && on) mat.EnableKeyword(EmissionKW);

                mat.SetColor(id, on ? highlightColor : original[r][m]);
            }
        }

        // Handle advanced floor circle
        if (useFloorCircle && floorCircle)
        {
            floorCircle.SetActive(on);
        }
    }

    // Public methods for runtime customization
    public void SetEnemyType(EnemyType newType)
    {
        enemyType = newType;
        if (circleMaterialInstance)
        {
            ConfigureShaderProperties();

            // Update scale
            if (floorCircle && enemyType)
                floorCircle.transform.localScale = Vector3.one * enemyType.circleRadius;
        }
    }

    public void SetCircleColors(Color primary, Color secondary)
    {
        if (circleMaterialInstance)
        {
            circleMaterialInstance.SetColor(GlowColorID, primary);
            circleMaterialInstance.SetColor(InnerGlowColorID, secondary);
        }
    }

    private void OnDestroy()
    {
        if (circleMaterialInstance)
            DestroyImmediate(circleMaterialInstance);
    }
}