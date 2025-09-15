using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public Image itemIconLarge;

    [Header("Tooltip Settings")]
    public Vector2 tooltipOffset = new Vector2(10, 10);

    private EnhancedInventorySlotUI slotUI;
    private Canvas canvas;

    private void Awake()
    {
        slotUI = GetComponent<EnhancedInventorySlotUI>();
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

        tooltipRect.anchoredPosition = canvasPosition + tooltipOffset;

        // Keep tooltip within screen bounds
        Vector3[] corners = new Vector3[4];
        tooltipRect.GetWorldCorners(corners);

        RectTransform canvasRect = canvas.transform as RectTransform;
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        // Adjust position if tooltip goes outside canvas
        Vector2 adjustment = Vector2.zero;

        if (corners[2].x > canvasCorners[2].x) // Right edge
            adjustment.x = canvasCorners[2].x - corners[2].x;
        if (corners[2].y > canvasCorners[2].y) // Top edge
            adjustment.y = canvasCorners[2].y - corners[2].y;
        if (corners[0].x < canvasCorners[0].x) // Left edge
            adjustment.x = canvasCorners[0].x - corners[0].x;
        if (corners[0].y < canvasCorners[0].y) // Bottom edge
            adjustment.y = canvasCorners[0].y - corners[0].y;

        tooltipRect.anchoredPosition += adjustment;
    }
}