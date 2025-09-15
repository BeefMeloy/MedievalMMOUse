using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("UI/Enhanced Target Frame UI")]
public class EnhancedTargetFrameUI : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private TargetManager targetManager;

    [Header("UI Layout")]
    [SerializeField] private GameObject root;
    [SerializeField] private RectTransform frameContainer;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image backgroundPanel;

    [Header("Frame Styling")]
    [SerializeField] private Image frameBorder;
    [SerializeField] private Image frameBackground;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject eliteIndicator;

    [Header("Layout Settings")]
    [SerializeField] private float minFrameWidth = 200f;
    [SerializeField] private float maxFrameWidth = 350f;
    [SerializeField] private float textPadding = 10f;
    [SerializeField] private bool autoSizeFrame = true;

    [Header("Animation")]
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private AnimationCurve showCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Highlightable current;
    private Targetable targetable;
    private Health health;
    private CanvasGroup canvasGroup;
    private float targetAlpha;
    private AudioSource audioSource;

    private void Awake()
    {
        if (!targetManager) targetManager = FindFirstObjectByType<TargetManager>();

        // FIXED: Always try to get CanvasGroup, create one if missing
        if (root)
        {
            canvasGroup = root.GetComponent<CanvasGroup>();
            if (!canvasGroup)
            {
                canvasGroup = root.AddComponent<CanvasGroup>();
                Debug.Log("Added CanvasGroup to target frame root");
            }
        }

        if (!audioSource) audioSource = GetComponent<AudioSource>();

        SetupComponents();

        // FIXED: Keep GameObject active, but make it invisible via CanvasGroup
        if (root)
        {
            root.SetActive(true);  // Keep active so it can receive updates
        }
    }

    private void SetupComponents()
    {
        if (healthBar)
        {
            healthBar.minValue = 0f;
            healthBar.maxValue = 1f;
        }

        if (nameText)
        {
            nameText.overflowMode = TextOverflowModes.Ellipsis;
            nameText.textWrappingMode = TMPro.TextWrappingModes.NoWrap;
        }

        // Initialize canvas group - start invisible
        if (canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            targetAlpha = 0f;  // Initialize target alpha to 0
        }
    }

    private void Update()
    {
        // Smooth alpha transitions
        if (canvasGroup && Mathf.Abs(canvasGroup.alpha - targetAlpha) > 0.01f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

            // Enable/disable raycasts based on visibility
            canvasGroup.blocksRaycasts = canvasGroup.alpha > 0.1f;
        }
    }

    private void LateUpdate()
    {
        if (!targetManager)
        {
            SetVisible(false);
            return;
        }

        var sel = targetManager.Selected;
        if (sel != current)
        {
            current = sel;

            if (!current)
            {
                SetVisible(false);
                targetable = null;
                health = null;
                return;
            }

            BindNewTarget();
        }

        UpdateDynamicElements();
    }

    private void BindNewTarget()
    {
        // Find components
        targetable = current.GetComponentInParent<Targetable>() ?? current.GetComponentInChildren<Targetable>();
        health = targetable ? targetable.health : current.GetComponentInParent<Health>() ?? current.GetComponentInChildren<Health>();

        SetVisible(true);
        UpdateStaticElements();
        UpdateLayout();
        PlayTargetSound();
    }

    private void UpdateStaticElements()
    {
        if (!targetable) return;

        // Enhanced name display with formatting
        if (nameText)
        {
            nameText.text = targetable.GetFullDisplayText();
        }

        // Level display
        if (levelText)
        {
            levelText.text = targetable.level.ToString();
            levelText.gameObject.SetActive(targetable.entityType && targetable.entityType.showLevel);
        }

        // Portrait
        if (portraitImage)
        {
            portraitImage.sprite = targetable.Portrait;
        }

        // Frame styling based on entity type
        UpdateFrameStyling();

        // Special indicators
        UpdateSpecialIndicators();
    }

    private void UpdateFrameStyling()
    {
        if (!targetable || !targetable.entityType) return;

        // Border color
        if (frameBorder)
        {
            frameBorder.color = targetable.FrameColor;
        }

        // Background tint
        if (frameBackground)
        {
            Color bgColor = targetable.FrameColor;
            bgColor.a = 0.1f; // Subtle tint
            frameBackground.color = bgColor;
        }

        // Health bar color
        if (healthBar)
        {
            ColorBlock colors = healthBar.colors;
            colors.normalColor = targetable.entityType.IsHostile() ? Color.red : Color.green;
            healthBar.colors = colors;
        }
    }

    private void UpdateSpecialIndicators()
    {
        if (!targetable || !eliteIndicator) return;

        bool isElite = targetable.entityType &&
                      (targetable.entityType.category == EntityCategory.Elite ||
                       targetable.entityType.category == EntityCategory.Boss);

        eliteIndicator.SetActive(isElite);
    }

    private void UpdateDynamicElements()
    {
        // Live HP update
        if (healthBar && health)
        {
            float targetValue = health.Max > 0 ? health.Normalized : 0f;
            healthBar.value = Mathf.Lerp(healthBar.value, targetValue, 10f * Time.deltaTime);
        }
    }

    private void UpdateLayout()
    {
        if (!autoSizeFrame || !frameContainer || !nameText) return;

        // Calculate needed width based on text
        float preferredWidth = nameText.preferredWidth + textPadding * 2;
        float targetWidth = Mathf.Clamp(preferredWidth, minFrameWidth, maxFrameWidth);

        // Smoothly resize frame
        Vector2 currentSize = frameContainer.sizeDelta;
        Vector2 targetSize = new Vector2(targetWidth, currentSize.y);
        frameContainer.sizeDelta = Vector2.Lerp(currentSize, targetSize, 8f * Time.deltaTime);
    }

    private void PlayTargetSound()
    {
        if (!audioSource || !targetable || !targetable.entityType || !targetable.entityType.targetSound) return;

        audioSource.PlayOneShot(targetable.entityType.targetSound);
    }

    private void SetVisible(bool visible)
    {
        targetAlpha = visible ? 1f : 0f;

        // FIXED: No longer using SetActive, only CanvasGroup alpha
        // The GameObject stays active so it can receive updates
    }
}