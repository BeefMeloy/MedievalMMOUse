using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;

    [Header("Inventory Reference")]
    public PlayerInventory playerInventory;

    private InventorySlotUI[] slotUIs;

    private void Start()
    {
        InitializeUI();

        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += UpdateSlotUI;
        }
    }

    private void InitializeUI()
    {
        foreach (Transform child in slotsParent)
        {
            DestroyImmediate(child.gameObject);
        }

        slotUIs = new InventorySlotUI[playerInventory.inventorySize];

        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Initialize(i, this);
                slotUIs[i] = slotUI;
            }
        }

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
        Debug.Log($"Slot {slotIndex} clicked");
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
}