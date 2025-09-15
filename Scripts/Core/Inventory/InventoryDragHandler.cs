using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  // Changed class name
{
    [Header("Drag Settings")]
    public Canvas canvas;
    public GraphicRaycaster graphicRaycaster;

    private InventorySlotUI slotUI;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject draggedIcon;

    private void Awake()
    {
        slotUI = GetComponent<InventorySlotUI>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (graphicRaycaster == null)
        {
            graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotUI.currentSlot == null || slotUI.currentSlot.IsEmpty())
            return;

        CreateDraggedIcon();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedIcon != null)
        {
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition);

            draggedIcon.transform.position = canvas.transform.TransformPoint(localPointerPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (draggedIcon != null)
        {
            Destroy(draggedIcon);
        }

        InventorySlotUI targetSlot = GetSlotUnderPointer(eventData);

        if (targetSlot != null && targetSlot != slotUI)
        {
            HandleItemDrop(targetSlot);
        }
    }

    private void CreateDraggedIcon()
    {
        draggedIcon = new GameObject("DraggedIcon");
        draggedIcon.transform.SetParent(canvas.transform, false);
        draggedIcon.transform.SetAsLastSibling();

        Image iconImage = draggedIcon.AddComponent<Image>();
        iconImage.sprite = slotUI.itemIcon.sprite;
        iconImage.raycastTarget = false;

        RectTransform draggedRect = draggedIcon.GetComponent<RectTransform>();
        draggedRect.sizeDelta = rectTransform.sizeDelta;
    }

    private InventorySlotUI GetSlotUnderPointer(PointerEventData eventData)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            InventorySlotUI slot = result.gameObject.GetComponent<InventorySlotUI>();
            if (slot != null)
                return slot;
        }

        return null;
    }

    private void HandleItemDrop(InventorySlotUI targetSlot)
    {
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null && inventoryUI.playerInventory != null)
        {
            inventoryUI.playerInventory.SwapSlots(slotUI.slotIndex, targetSlot.slotIndex);
        }
    }
}