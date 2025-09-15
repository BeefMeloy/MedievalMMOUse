using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhancedUISetup : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Canvas targetCanvas;

    [ContextMenu("Create Enhanced Target Frame")]
    public void CreateEnhancedTargetFrame()
    {
        if (!targetCanvas)
        {
            Debug.LogError("Please assign a Canvas first!");
            return;
        }

        // Create the main target frame
        GameObject targetFrame = CreateTargetFrameStructure();

        // Add the EnhancedTargetFrameUI component
        var frameUI = targetFrame.AddComponent<EnhancedTargetFrameUI>();

        // Auto-assign all the UI references
        SetupFrameUIReferences(frameUI, targetFrame);

        Debug.Log("Enhanced Target Frame created successfully!");
        Debug.Log("Please manually assign the remaining UI references in EnhancedTargetFrameUI component.");
    }

    private GameObject CreateTargetFrameStructure()
    {
        // Main target frame container
        GameObject targetFrame = new GameObject("Enhanced Target Frame");
        targetFrame.transform.SetParent(targetCanvas.transform, false);

        var frameRect = targetFrame.AddComponent<RectTransform>();
        frameRect.anchorMin = new Vector2(0, 1);
        frameRect.anchorMax = new Vector2(0, 1);
        frameRect.anchoredPosition = new Vector2(20, -20);
        frameRect.sizeDelta = new Vector2(320, 90);

        // Add CanvasGroup for fading
        var canvasGroup = targetFrame.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        // Background Panel
        GameObject backgroundPanel = new GameObject("Background Panel");
        backgroundPanel.transform.SetParent(targetFrame.transform, false);
        var bgImage = backgroundPanel.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f);
        var bgRect = backgroundPanel.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Frame Border
        GameObject frameBorder = new GameObject("Frame Border");
        frameBorder.transform.SetParent(targetFrame.transform, false);
        var borderImage = frameBorder.AddComponent<Image>();
        borderImage.color = Color.red;
        var borderRect = frameBorder.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(-3, -3);
        borderRect.offsetMax = new Vector2(3, 3);

        // Content Container
        GameObject contentContainer = new GameObject("Content Container");
        contentContainer.transform.SetParent(targetFrame.transform, false);
        var contentRect = contentContainer.AddComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(8, 8);
        contentRect.offsetMax = new Vector2(-8, -8);

        // Add Horizontal Layout Group
        var horizontalLayout = contentContainer.AddComponent<HorizontalLayoutGroup>();
        horizontalLayout.spacing = 12;
        horizontalLayout.padding = new RectOffset(5, 5, 5, 5);
        horizontalLayout.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayout.childControlWidth = false;
        horizontalLayout.childControlHeight = false;

        // Portrait Section
        GameObject portraitSection = CreatePortraitSection(contentContainer);

        // Info Section
        GameObject infoSection = CreateInfoSection(contentContainer);

        // Elite Indicator
        GameObject eliteIndicator = new GameObject("Elite Indicator");
        eliteIndicator.transform.SetParent(targetFrame.transform, false);
        var eliteImage = eliteIndicator.AddComponent<Image>();
        eliteImage.color = Color.gold;
        var eliteRect = eliteIndicator.GetComponent<RectTransform>();
        eliteRect.anchorMin = new Vector2(1, 1);
        eliteRect.anchorMax = new Vector2(1, 1);
        eliteRect.anchoredPosition = new Vector2(-15, -15);
        eliteRect.sizeDelta = new Vector2(24, 24);
        eliteIndicator.SetActive(false);

        return targetFrame;
    }

    private GameObject CreatePortraitSection(GameObject parent)
    {
        GameObject portraitSection = new GameObject("Portrait Section");
        portraitSection.transform.SetParent(parent.transform, false);
        var portraitRect = portraitSection.AddComponent<RectTransform>();
        portraitRect.sizeDelta = new Vector2(64, 64);

        // Portrait Image
        GameObject portraitImage = new GameObject("Portrait Image");
        portraitImage.transform.SetParent(portraitSection.transform, false);
        var image = portraitImage.AddComponent<Image>();
        image.color = Color.gray; // Default color
        var imageRect = portraitImage.GetComponent<RectTransform>();
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;

        return portraitSection;
    }

    private GameObject CreateInfoSection(GameObject parent)
    {
        GameObject infoSection = new GameObject("Info Section");
        infoSection.transform.SetParent(parent.transform, false);
        var infoRect = infoSection.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(220, 64);

        // Add Vertical Layout Group
        var verticalLayout = infoSection.AddComponent<VerticalLayoutGroup>();
        verticalLayout.spacing = 3;
        verticalLayout.childAlignment = TextAnchor.UpperLeft;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;

        // Name Text (Main name with level)
        GameObject nameText = CreateTextMeshPro("Name Text", infoSection);
        var nameComponent = nameText.GetComponent<TextMeshProUGUI>();
        nameComponent.text = "Target Name [15]";
        nameComponent.fontSize = 16;
        nameComponent.fontStyle = FontStyles.Bold;
        nameComponent.color = Color.white;

        // Guild Text
        GameObject guildText = CreateTextMeshPro("Guild Text", infoSection);
        var guildComponent = guildText.GetComponent<TextMeshProUGUI>();
        guildComponent.text = "<Guild Name>";
        guildComponent.fontSize = 11;
        guildComponent.color = Color.gray;
        guildComponent.fontStyle = FontStyles.Italic;

        // Health Bar
        GameObject healthBar = CreateHealthBar(infoSection);

        return infoSection;
    }

    private GameObject CreateTextMeshPro(string name, GameObject parent)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent.transform, false);

        var textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = name;
        textComponent.fontSize = 14;
        textComponent.color = Color.white;
        textComponent.overflowMode = TextOverflowModes.Ellipsis;
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;

        var rect = textObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 22);

        return textObject;
    }

    private GameObject CreateHealthBar(GameObject parent)
    {
        GameObject healthBarObject = new GameObject("Health Bar");
        healthBarObject.transform.SetParent(parent.transform, false);

        var healthRect = healthBarObject.GetComponent<RectTransform>();
        healthRect.sizeDelta = new Vector2(200, 10);

        var slider = healthBarObject.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.85f;
        slider.transition = Selectable.Transition.None;

        // Background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(healthBarObject.transform, false);
        var bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 1);
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Fill Area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(healthBarObject.transform, false);
        var fillAreaRect = fillArea.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = Vector2.zero;
        fillAreaRect.offsetMax = Vector2.zero;

        // Fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.green;
        var fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        slider.targetGraphic = fillImage;
        slider.fillRect = fillRect;

        return healthBarObject;
    }

    private void SetupFrameUIReferences(EnhancedTargetFrameUI frameUI, GameObject targetFrame)
    {
        // The script will need manual assignment since Unity's SerializeField 
        // references can't be set via code easily
        Debug.Log("Target Frame UI created! You'll need to manually assign references:");
        Debug.Log("- Root: " + targetFrame.name);
        Debug.Log("- Frame Container: Content Container");
        Debug.Log("- Portrait Image: Portrait Section/Portrait Image");
        Debug.Log("- Name Text: Info Section/Name Text");
        Debug.Log("- Health Bar: Info Section/Health Bar");
        Debug.Log("- Frame Border: Frame Border");
        Debug.Log("- Elite Indicator: Elite Indicator");
    }
}