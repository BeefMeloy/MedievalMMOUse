using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public Image slotBackground;

    [Header("Visual Settings")]
    public Color emptySlotColor = new Color(1f, 1f, 1f, 0f); // Completely transparent
    public Color filledSlotColor = new Color(1f, 1f, 1f, 0.1f); // Slightly visible when filled
    public Color hoverColor = new Color(1f, 1f, 1f, 0.3f); // Highlight on hover

    public int slotIndex { get; private set; }
    private InventoryUI inventoryUI;
    public InventorySlot currentSlot { get; private set; }

    public void Initialize(int index, InventoryUI invUI)
    {
        slotIndex = index;
        inventoryUI = invUI;

        // Make background transparent by default
        if (slotBackground != null)
        {
            slotBackground.color = emptySlotColor;
        }

        UpdateSlot(null);
    }

    public void UpdateSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot == null || slot.IsEmpty())
        {
            // Empty slot - completely transparent
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.color = new Color(1f, 1f, 1f, 0f); // Invisible
            }
            if (quantityText != null)
                quantityText.text = "";
            if (slotBackground != null)
                slotBackground.color = emptySlotColor; // Transparent
        }
        else
        {
            // Filled slot - show item
            if (itemIcon != null)
            {
                itemIcon.sprite = slot.item.icon;
                itemIcon.color = Color.white; // Fully visible
            }
            if (slotBackground != null)
                slotBackground.color = filledSlotColor; // Slightly visible background

            if (slot.item.isStackable && slot.quantity > 1)
            {
                if (quantityText != null)
                    quantityText.text = slot.quantity.ToString();
            }
            else
            {
                if (quantityText != null)
                    quantityText.text = "";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.OnSlotClicked(slotIndex);
    }
}