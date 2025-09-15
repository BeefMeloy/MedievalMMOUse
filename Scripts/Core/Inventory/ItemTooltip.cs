<<<<<<< HEAD
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public Image itemIconLarge;

    private InventorySlotUI slotUI;
    private Canvas canvas;

    private void Awake()
    {
        slotUI = GetComponent<InventorySlotUI>();
        canvas = GetComponentInParent<Canvas>();

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotUI.currentSlot != null && !slotUI.currentSlot.IsEmpty())
        {
            ShowTooltip(slotUI.currentSlot.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip(Item item)
    {
        if (tooltipPanel == null) return;

        tooltipPanel.SetActive(true);

        // Update tooltip content
        if (itemNameText != null)
            itemNameText.text = item.itemName;

        if (itemDescriptionText != null)
            itemDescriptionText.text = item.description;

        if (itemStatsText != null)
        {
            string stats = $"Value: {item.value}\n";
            stats += $"Weight: {item.weight}\n";
            stats += $"Type: {item.itemType}";
            if (item.isStackable)
                stats += $"\nMax Stack: {item.maxStackSize}";
            itemStatsText.text = stats;
        }

        if (itemIconLarge != null)
            itemIconLarge.sprite = item.icon;

        // Position tooltip near mouse
        PositionTooltip();
    }

    private void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    private void PositionTooltip()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();

        // Convert mouse position to canvas space
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out canvasPosition);

        tooltipRect.anchoredPosition = canvasPosition + new Vector2(10, 10);
    }
=======
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public Image itemIconLarge;

    private InventorySlotUI slotUI;
    private Canvas canvas;

    private void Awake()
    {
        slotUI = GetComponent<InventorySlotUI>();
        canvas = GetComponentInParent<Canvas>();

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotUI.currentSlot != null && !slotUI.currentSlot.IsEmpty())
        {
            ShowTooltip(slotUI.currentSlot.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip(Item item)
    {
        if (tooltipPanel == null) return;

        tooltipPanel.SetActive(true);

        // Update tooltip content
        if (itemNameText != null)
            itemNameText.text = item.itemName;

        if (itemDescriptionText != null)
            itemDescriptionText.text = item.description;

        if (itemStatsText != null)
        {
            string stats = $"Value: {item.value}\n";
            stats += $"Weight: {item.weight}\n";
            stats += $"Type: {item.itemType}";
            if (item.isStackable)
                stats += $"\nMax Stack: {item.maxStackSize}";
            itemStatsText.text = stats;
        }

        if (itemIconLarge != null)
            itemIconLarge.sprite = item.icon;

        // Position tooltip near mouse
        PositionTooltip();
    }

    private void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    private void PositionTooltip()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();

        // Convert mouse position to canvas space
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out canvasPosition);

        tooltipRect.anchoredPosition = canvasPosition + new Vector2(10, 10);
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}