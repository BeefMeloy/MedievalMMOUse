<<<<<<< HEAD
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnhancedInventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public Image slotBackground;

    [Header("Visual Settings")]
    public Color emptySlotColor = new Color(1f, 1f, 1f, 0.3f);
    public Color filledSlotColor = Color.white;

    public int slotIndex { get; private set; }  // Made public for drag handler
    private EnhancedInventoryUI inventoryUI;
    public InventorySlot currentSlot { get; private set; }

    public void Initialize(int index, EnhancedInventoryUI invUI)
    {
        slotIndex = index;
        inventoryUI = invUI;

        // Set initial state
        UpdateSlot(null);
    }

    public void UpdateSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot == null || slot.IsEmpty())
        {
            // Empty slot
            itemIcon.sprite = null;
            itemIcon.color = emptySlotColor;
            quantityText.text = "";
            slotBackground.color = emptySlotColor;
        }
        else
        {
            // Filled slot
            itemIcon.sprite = slot.item.icon;
            itemIcon.color = filledSlotColor;
            slotBackground.color = filledSlotColor;

            // Show quantity if stackable and more than 1
            if (slot.item.isStackable && slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();
            }
            else
            {
                quantityText.text = "";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.OnSlotClicked(slotIndex);
    }
=======
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnhancedInventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public Image slotBackground;

    [Header("Visual Settings")]
    public Color emptySlotColor = new Color(1f, 1f, 1f, 0.3f);
    public Color filledSlotColor = Color.white;

    public int slotIndex { get; private set; }  // Made public for drag handler
    private EnhancedInventoryUI inventoryUI;
    public InventorySlot currentSlot { get; private set; }

    public void Initialize(int index, EnhancedInventoryUI invUI)
    {
        slotIndex = index;
        inventoryUI = invUI;

        // Set initial state
        UpdateSlot(null);
    }

    public void UpdateSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot == null || slot.IsEmpty())
        {
            // Empty slot
            itemIcon.sprite = null;
            itemIcon.color = emptySlotColor;
            quantityText.text = "";
            slotBackground.color = emptySlotColor;
        }
        else
        {
            // Filled slot
            itemIcon.sprite = slot.item.icon;
            itemIcon.color = filledSlotColor;
            slotBackground.color = filledSlotColor;

            // Show quantity if stackable and more than 1
            if (slot.item.isStackable && slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();
            }
            else
            {
                quantityText.text = "";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.OnSlotClicked(slotIndex);
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}