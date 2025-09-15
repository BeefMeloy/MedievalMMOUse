<<<<<<< HEAD
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUISetup : MonoBehaviour
{
    [Header("Setup")]
    public Canvas gameCanvas;

    [Header("Manual Execution")]
    public bool createUI = false;

    private void Update()
    {
        if (createUI)
        {
            createUI = false;
            CreateCompleteGameUI();
        }
    }

    [ContextMenu("Create Complete Game UI")]
    public void CreateCompleteGameUI()
    {
        if (!gameCanvas)
        {
            Debug.LogError("Please assign the Game Canvas first!");
            return;
        }

        CreatePlayerFrame();
        CreateEnemyTargetFrame();

        Debug.Log("Complete Game UI created successfully!");
    }

    private void CreatePlayerFrame()
    {
        GameObject playerFrame = new GameObject("Player Frame");
        playerFrame.transform.SetParent(gameCanvas.transform, false);

        var playerRect = playerFrame.AddComponent<RectTransform>();
        playerRect.anchorMin = new Vector2(0, 1);
        playerRect.anchorMax = new Vector2(0, 1);
        playerRect.anchoredPosition = new Vector2(20, -20);
        playerRect.sizeDelta = new Vector2(280, 80);

        var playerBg = playerFrame.AddComponent<Image>();
        playerBg.color = new Color(0, 0, 0, 0.8f);

        GameObject playerContent = new GameObject("Player Content");
        playerContent.transform.SetParent(playerFrame.transform, false);
        var contentRect = playerContent.AddComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(8, 8);
        contentRect.offsetMax = new Vector2(-8, -8);

        var horizontalLayout = playerContent.AddComponent<HorizontalLayoutGroup>();
        horizontalLayout.spacing = 12;
        horizontalLayout.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayout.childControlWidth = false;
        horizontalLayout.childControlHeight = false;

        GameObject playerPortrait = new GameObject("Player Portrait");
        playerPortrait.transform.SetParent(playerContent.transform, false);
        var portraitRect = playerPortrait.AddComponent<RectTransform>();
        portraitRect.sizeDelta = new Vector2(64, 64);

        var portraitImage = playerPortrait.AddComponent<Image>();
        portraitImage.color = new Color(0.3f, 0.6f, 0.3f, 1f);

        GameObject playerInfo = new GameObject("Player Info");
        playerInfo.transform.SetParent(playerContent.transform, false);
        var infoRect = playerInfo.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(180, 64);

        var verticalLayout = playerInfo.AddComponent<VerticalLayoutGroup>();
        verticalLayout.spacing = 2;
        verticalLayout.childAlignment = TextAnchor.UpperLeft;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;

        CreatePlayerText("Player Name", playerInfo, "Hiro", 14, FontStyles.Bold, Color.white);
        CreateStatusBar("Health Bar", playerInfo, new Color(0, 0.8f, 0, 1f), "Health 164/164");
        CreateStatusBar("Mana Bar", playerInfo, new Color(0, 0.4f, 0.8f, 1f), "Mana 94/94");

        Debug.Log("Created Player Frame");
    }

    private void CreateEnemyTargetFrame()
    {
        GameObject enemyFrame = new GameObject("Enemy Target Frame");
        enemyFrame.transform.SetParent(gameCanvas.transform, false);

        var enemyRect = enemyFrame.AddComponent<RectTransform>();
        enemyRect.anchorMin = new Vector2(1, 1);
        enemyRect.anchorMax = new Vector2(1, 1);
        enemyRect.anchoredPosition = new Vector2(-20, -20);
        enemyRect.sizeDelta = new Vector2(300, 80);

        var canvasGroup = enemyFrame.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        var enemyBg = enemyFrame.AddComponent<Image>();
        enemyBg.color = new Color(0, 0, 0, 0.8f);

        GameObject frameBorder = new GameObject("Frame Border");
        frameBorder.transform.SetParent(enemyFrame.transform, false);
        var borderImage = frameBorder.AddComponent<Image>();
        borderImage.color = Color.red;
        var borderRect = frameBorder.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(-3, -3);
        borderRect.offsetMax = new Vector2(3, 3);

        GameObject enemyContent = new GameObject("Enemy Content");
        enemyContent.transform.SetParent(enemyFrame.transform, false);
        var contentRect = enemyContent.AddComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(8, 8);
        contentRect.offsetMax = new Vector2(-8, -8);

        var horizontalLayout = enemyContent.AddComponent<HorizontalLayoutGroup>();
        horizontalLayout.spacing = 12;
        horizontalLayout.childAlignment = TextAnchor.MiddleRight;
        horizontalLayout.childControlWidth = false;
        horizontalLayout.childControlHeight = false;
        horizontalLayout.reverseArrangement = true;

        GameObject enemyPortrait = new GameObject("Enemy Portrait");
        enemyPortrait.transform.SetParent(enemyContent.transform, false);
        var portraitRect = enemyPortrait.AddComponent<RectTransform>();
        portraitRect.sizeDelta = new Vector2(64, 64);

        var portraitImage = enemyPortrait.AddComponent<Image>();
        portraitImage.color = new Color(0.6f, 0.3f, 0.3f, 1f);

        GameObject enemyInfo = new GameObject("Enemy Info");
        enemyInfo.transform.SetParent(enemyContent.transform, false);
        var infoRect = enemyInfo.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(200, 64);

        var verticalLayout = enemyInfo.AddComponent<VerticalLayoutGroup>();
        verticalLayout.spacing = 3;
        verticalLayout.childAlignment = TextAnchor.UpperRight;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;

        CreateEnemyText("Enemy Name", enemyInfo, "Goblin Warrior [5]", 16, FontStyles.Bold, Color.white);
        CreateEnemyText("Guild Text", enemyInfo, "<Goblin Tribe>", 11, FontStyles.Italic, Color.gray);
        CreateEnemyStatusBar("Enemy Health Bar", enemyInfo, Color.red);

        GameObject eliteIndicator = new GameObject("Elite Indicator");
        eliteIndicator.transform.SetParent(enemyFrame.transform, false);
        var eliteImage = eliteIndicator.AddComponent<Image>();
        eliteImage.color = Color.gold;
        var eliteRect = eliteIndicator.GetComponent<RectTransform>();
        eliteRect.anchorMin = new Vector2(1, 1);
        eliteRect.anchorMax = new Vector2(1, 1);
        eliteRect.anchoredPosition = new Vector2(-15, -15);
        eliteRect.sizeDelta = new Vector2(20, 20);
        eliteIndicator.SetActive(false);

        // Add component and set root reference
        var enemyUI = enemyFrame.AddComponent<EnhancedTargetFrameUI>();

        // FIXED: Use reflection to set the root field
        var rootField = typeof(EnhancedTargetFrameUI).GetField("root",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rootField != null)
        {
            rootField.SetValue(enemyUI, enemyFrame);
            Debug.Log("Auto-assigned root reference for EnhancedTargetFrameUI");
        }

        Debug.Log("Created Enemy Frame - manually assign remaining UI references");
    }

    private GameObject CreatePlayerText(string name, GameObject parent, string text, int fontSize, FontStyles style, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added
        var rect = textObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(180, 18);

        var textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.fontStyle = style;
        textComponent.color = color;
        textComponent.alignment = TextAlignmentOptions.Left;

        return textObj;
    }

    private GameObject CreateEnemyText(string name, GameObject parent, string text, int fontSize, FontStyles style, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added
        var rect = textObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(190, 18);

        var textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.fontStyle = style;
        textComponent.color = color;
        textComponent.alignment = TextAlignmentOptions.Right;

        return textObj;
    }

    private GameObject CreateStatusBar(string name, GameObject parent, Color barColor, string labelText)
    {
        GameObject barContainer = new GameObject(name);
        barContainer.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added FIRST
        var containerRect = barContainer.AddComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(180, 12);

        GameObject background = new GameObject("Background");
        background.transform.SetParent(barContainer.transform, false);
        var bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        var slider = barContainer.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1f;
        slider.transition = Selectable.Transition.None;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(barContainer.transform, false);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = barColor;
        var fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        slider.fillRect = fillRect;

        GameObject label = new GameObject("Label");
        label.transform.SetParent(barContainer.transform, false);
        var labelText_component = label.AddComponent<TextMeshProUGUI>();
        labelText_component.text = labelText;
        labelText_component.fontSize = 10;
        labelText_component.color = Color.white;
        labelText_component.alignment = TextAlignmentOptions.Center;
        labelText_component.fontStyle = FontStyles.Bold;
        var labelRect = label.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        return barContainer;
    }

    private GameObject CreateEnemyStatusBar(string name, GameObject parent, Color barColor)
    {
        GameObject barContainer = new GameObject(name);
        barContainer.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added FIRST
        var containerRect = barContainer.AddComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(190, 10);

        GameObject background = new GameObject("Background");
        background.transform.SetParent(barContainer.transform, false);
        var bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        var slider = barContainer.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.8f;
        slider.transition = Selectable.Transition.None;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(barContainer.transform, false);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = barColor;
        var fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        slider.fillRect = fillRect;

        return barContainer;
    }
=======
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUISetup : MonoBehaviour
{
    [Header("Setup")]
    public Canvas gameCanvas;

    [Header("Manual Execution")]
    public bool createUI = false;

    private void Update()
    {
        if (createUI)
        {
            createUI = false;
            CreateCompleteGameUI();
        }
    }

    [ContextMenu("Create Complete Game UI")]
    public void CreateCompleteGameUI()
    {
        if (!gameCanvas)
        {
            Debug.LogError("Please assign the Game Canvas first!");
            return;
        }

        CreatePlayerFrame();
        CreateEnemyTargetFrame();

        Debug.Log("Complete Game UI created successfully!");
    }

    private void CreatePlayerFrame()
    {
        GameObject playerFrame = new GameObject("Player Frame");
        playerFrame.transform.SetParent(gameCanvas.transform, false);

        var playerRect = playerFrame.AddComponent<RectTransform>();
        playerRect.anchorMin = new Vector2(0, 1);
        playerRect.anchorMax = new Vector2(0, 1);
        playerRect.anchoredPosition = new Vector2(20, -20);
        playerRect.sizeDelta = new Vector2(280, 80);

        var playerBg = playerFrame.AddComponent<Image>();
        playerBg.color = new Color(0, 0, 0, 0.8f);

        GameObject playerContent = new GameObject("Player Content");
        playerContent.transform.SetParent(playerFrame.transform, false);
        var contentRect = playerContent.AddComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(8, 8);
        contentRect.offsetMax = new Vector2(-8, -8);

        var horizontalLayout = playerContent.AddComponent<HorizontalLayoutGroup>();
        horizontalLayout.spacing = 12;
        horizontalLayout.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayout.childControlWidth = false;
        horizontalLayout.childControlHeight = false;

        GameObject playerPortrait = new GameObject("Player Portrait");
        playerPortrait.transform.SetParent(playerContent.transform, false);
        var portraitRect = playerPortrait.AddComponent<RectTransform>();
        portraitRect.sizeDelta = new Vector2(64, 64);

        var portraitImage = playerPortrait.AddComponent<Image>();
        portraitImage.color = new Color(0.3f, 0.6f, 0.3f, 1f);

        GameObject playerInfo = new GameObject("Player Info");
        playerInfo.transform.SetParent(playerContent.transform, false);
        var infoRect = playerInfo.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(180, 64);

        var verticalLayout = playerInfo.AddComponent<VerticalLayoutGroup>();
        verticalLayout.spacing = 2;
        verticalLayout.childAlignment = TextAnchor.UpperLeft;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;

        CreatePlayerText("Player Name", playerInfo, "Hiro", 14, FontStyles.Bold, Color.white);
        CreateStatusBar("Health Bar", playerInfo, new Color(0, 0.8f, 0, 1f), "Health 164/164");
        CreateStatusBar("Mana Bar", playerInfo, new Color(0, 0.4f, 0.8f, 1f), "Mana 94/94");

        Debug.Log("Created Player Frame");
    }

    private void CreateEnemyTargetFrame()
    {
        GameObject enemyFrame = new GameObject("Enemy Target Frame");
        enemyFrame.transform.SetParent(gameCanvas.transform, false);

        var enemyRect = enemyFrame.AddComponent<RectTransform>();
        enemyRect.anchorMin = new Vector2(1, 1);
        enemyRect.anchorMax = new Vector2(1, 1);
        enemyRect.anchoredPosition = new Vector2(-20, -20);
        enemyRect.sizeDelta = new Vector2(300, 80);

        var canvasGroup = enemyFrame.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        var enemyBg = enemyFrame.AddComponent<Image>();
        enemyBg.color = new Color(0, 0, 0, 0.8f);

        GameObject frameBorder = new GameObject("Frame Border");
        frameBorder.transform.SetParent(enemyFrame.transform, false);
        var borderImage = frameBorder.AddComponent<Image>();
        borderImage.color = Color.red;
        var borderRect = frameBorder.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(-3, -3);
        borderRect.offsetMax = new Vector2(3, 3);

        GameObject enemyContent = new GameObject("Enemy Content");
        enemyContent.transform.SetParent(enemyFrame.transform, false);
        var contentRect = enemyContent.AddComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(8, 8);
        contentRect.offsetMax = new Vector2(-8, -8);

        var horizontalLayout = enemyContent.AddComponent<HorizontalLayoutGroup>();
        horizontalLayout.spacing = 12;
        horizontalLayout.childAlignment = TextAnchor.MiddleRight;
        horizontalLayout.childControlWidth = false;
        horizontalLayout.childControlHeight = false;
        horizontalLayout.reverseArrangement = true;

        GameObject enemyPortrait = new GameObject("Enemy Portrait");
        enemyPortrait.transform.SetParent(enemyContent.transform, false);
        var portraitRect = enemyPortrait.AddComponent<RectTransform>();
        portraitRect.sizeDelta = new Vector2(64, 64);

        var portraitImage = enemyPortrait.AddComponent<Image>();
        portraitImage.color = new Color(0.6f, 0.3f, 0.3f, 1f);

        GameObject enemyInfo = new GameObject("Enemy Info");
        enemyInfo.transform.SetParent(enemyContent.transform, false);
        var infoRect = enemyInfo.AddComponent<RectTransform>();
        infoRect.sizeDelta = new Vector2(200, 64);

        var verticalLayout = enemyInfo.AddComponent<VerticalLayoutGroup>();
        verticalLayout.spacing = 3;
        verticalLayout.childAlignment = TextAnchor.UpperRight;
        verticalLayout.childControlWidth = true;
        verticalLayout.childControlHeight = false;

        CreateEnemyText("Enemy Name", enemyInfo, "Goblin Warrior [5]", 16, FontStyles.Bold, Color.white);
        CreateEnemyText("Guild Text", enemyInfo, "<Goblin Tribe>", 11, FontStyles.Italic, Color.gray);
        CreateEnemyStatusBar("Enemy Health Bar", enemyInfo, Color.red);

        GameObject eliteIndicator = new GameObject("Elite Indicator");
        eliteIndicator.transform.SetParent(enemyFrame.transform, false);
        var eliteImage = eliteIndicator.AddComponent<Image>();
        eliteImage.color = Color.gold;
        var eliteRect = eliteIndicator.GetComponent<RectTransform>();
        eliteRect.anchorMin = new Vector2(1, 1);
        eliteRect.anchorMax = new Vector2(1, 1);
        eliteRect.anchoredPosition = new Vector2(-15, -15);
        eliteRect.sizeDelta = new Vector2(20, 20);
        eliteIndicator.SetActive(false);

        // Add component and set root reference
        var enemyUI = enemyFrame.AddComponent<EnhancedTargetFrameUI>();

        // FIXED: Use reflection to set the root field
        var rootField = typeof(EnhancedTargetFrameUI).GetField("root",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rootField != null)
        {
            rootField.SetValue(enemyUI, enemyFrame);
            Debug.Log("Auto-assigned root reference for EnhancedTargetFrameUI");
        }

        Debug.Log("Created Enemy Frame - manually assign remaining UI references");
    }

    private GameObject CreatePlayerText(string name, GameObject parent, string text, int fontSize, FontStyles style, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added
        var rect = textObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(180, 18);

        var textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.fontStyle = style;
        textComponent.color = color;
        textComponent.alignment = TextAlignmentOptions.Left;

        return textObj;
    }

    private GameObject CreateEnemyText(string name, GameObject parent, string text, int fontSize, FontStyles style, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added
        var rect = textObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(190, 18);

        var textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.fontStyle = style;
        textComponent.color = color;
        textComponent.alignment = TextAlignmentOptions.Right;

        return textObj;
    }

    private GameObject CreateStatusBar(string name, GameObject parent, Color barColor, string labelText)
    {
        GameObject barContainer = new GameObject(name);
        barContainer.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added FIRST
        var containerRect = barContainer.AddComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(180, 12);

        GameObject background = new GameObject("Background");
        background.transform.SetParent(barContainer.transform, false);
        var bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        var slider = barContainer.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1f;
        slider.transition = Selectable.Transition.None;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(barContainer.transform, false);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = barColor;
        var fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        slider.fillRect = fillRect;

        GameObject label = new GameObject("Label");
        label.transform.SetParent(barContainer.transform, false);
        var labelText_component = label.AddComponent<TextMeshProUGUI>();
        labelText_component.text = labelText;
        labelText_component.fontSize = 10;
        labelText_component.color = Color.white;
        labelText_component.alignment = TextAlignmentOptions.Center;
        labelText_component.fontStyle = FontStyles.Bold;
        var labelRect = label.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        return barContainer;
    }

    private GameObject CreateEnemyStatusBar(string name, GameObject parent, Color barColor)
    {
        GameObject barContainer = new GameObject(name);
        barContainer.transform.SetParent(parent.transform, false);

        // FIXED: Ensure RectTransform is added FIRST
        var containerRect = barContainer.AddComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(190, 10);

        GameObject background = new GameObject("Background");
        background.transform.SetParent(barContainer.transform, false);
        var bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        var slider = barContainer.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.8f;
        slider.transition = Selectable.Transition.None;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(barContainer.transform, false);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = barColor;
        var fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        slider.fillRect = fillRect;

        return barContainer;
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}