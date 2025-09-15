<<<<<<< HEAD
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhancedInventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;

    [Header("Inventory Reference")]
    public PlayerInventory playerInventory;

    private EnhancedInventorySlotUI[] slotUIs;

    private void Start()
    {
        InitializeUI();

        if (playerInventory != null)
        {
            // Subscribe to inventory changes
            playerInventory.OnInventoryChanged += UpdateSlotUI;
        }
    }

    private void InitializeUI()
    {
        // Clear existing slots
        foreach (Transform child in slotsParent)
        {
            DestroyImmediate(child.gameObject);
        }

        // Create slot UIs
        slotUIs = new EnhancedInventorySlotUI[playerInventory.inventorySize];

        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            EnhancedInventorySlotUI slotUI = slotObj.GetComponent<EnhancedInventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Initialize(i, this);
                slotUIs[i] = slotUI;
            }
        }

        // Update all slots initially
        UpdateAllSlots();
    }

    private void UpdateSlotUI(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotUIs.Length && slotUIs[slotIndex] != null)
        {
            InventorySlot slot = playerInventory.GetSlot(slotIndex);
            slotUIs[slotIndex].UpdateSlot(slot);
        }
    }

    private void UpdateAllSlots()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            UpdateSlotUI(i);
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        // Handle slot clicking (for item interaction, moving, etc.)
        Debug.Log($"Slot {slotIndex} clicked");

        // You can add logic here for:
        // - Item dragging and dropping
        // - Item usage
        // - Item information display
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateSlotUI;
        }
    }
=======
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhancedInventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;

    [Header("Inventory Reference")]
    public PlayerInventory playerInventory;

    private EnhancedInventorySlotUI[] slotUIs;

    private void Start()
    {
        InitializeUI();

        if (playerInventory != null)
        {
            // Subscribe to inventory changes
            playerInventory.OnInventoryChanged += UpdateSlotUI;
        }
    }

    private void InitializeUI()
    {
        // Clear existing slots
        foreach (Transform child in slotsParent)
        {
            DestroyImmediate(child.gameObject);
        }

        // Create slot UIs
        slotUIs = new EnhancedInventorySlotUI[playerInventory.inventorySize];

        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            EnhancedInventorySlotUI slotUI = slotObj.GetComponent<EnhancedInventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Initialize(i, this);
                slotUIs[i] = slotUI;
            }
        }

        // Update all slots initially
        UpdateAllSlots();
    }

    private void UpdateSlotUI(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotUIs.Length && slotUIs[slotIndex] != null)
        {
            InventorySlot slot = playerInventory.GetSlot(slotIndex);
            slotUIs[slotIndex].UpdateSlot(slot);
        }
    }

    private void UpdateAllSlots()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            UpdateSlotUI(i);
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        // Handle slot clicking (for item interaction, moving, etc.)
        Debug.Log($"Slot {slotIndex} clicked");

        // You can add logic here for:
        // - Item dragging and dropping
        // - Item usage
        // - Item information display
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateSlotUI;
        }
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}