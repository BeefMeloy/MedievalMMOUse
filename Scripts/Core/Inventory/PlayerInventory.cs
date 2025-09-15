using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int inventorySize = 28; // Changed from 32 to 28

    [SerializeField]
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    [Header("Events")]
    public System.Action<int> OnInventoryChanged;
    public System.Action<Item, int> OnItemAdded;
    public System.Action<Item, int> OnItemRemoved;

    private void Awake()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        inventorySlots.Clear();
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    // Rest of your methods remain the same...
    public bool AddItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        int remainingQuantity = quantity;

        // First, try to stack with existing items
        if (item.isStackable)
        {
            for (int i = 0; i < inventorySlots.Count && remainingQuantity > 0; i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (!slot.IsEmpty() && slot.item == item)
                {
                    int canAdd = Mathf.Min(remainingQuantity, item.maxStackSize - slot.quantity);
                    if (canAdd > 0)
                    {
                        slot.AddItem(item, canAdd);
                        remainingQuantity -= canAdd;
                        OnInventoryChanged?.Invoke(i);
                    }
                }
            }
        }

        // Then, use empty slots
        for (int i = 0; i < inventorySlots.Count && remainingQuantity > 0; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.IsEmpty())
            {
                int toAdd = item.isStackable ? Mathf.Min(remainingQuantity, item.maxStackSize) : 1;
                slot.AddItem(item, toAdd);
                remainingQuantity -= toAdd;
                OnInventoryChanged?.Invoke(i);
            }
        }

        if (remainingQuantity < quantity)
        {
            OnItemAdded?.Invoke(item, quantity - remainingQuantity);
            return remainingQuantity == 0;
        }

        return false;
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        int remainingToRemove = quantity;

        for (int i = inventorySlots.Count - 1; i >= 0 && remainingToRemove > 0; i--)
        {
            InventorySlot slot = inventorySlots[i];
            if (!slot.IsEmpty() && slot.item == item)
            {
                int toRemove = Mathf.Min(remainingToRemove, slot.quantity);
                slot.RemoveItem(toRemove);
                remainingToRemove -= toRemove;
                OnInventoryChanged?.Invoke(i);
            }
        }

        if (remainingToRemove < quantity)
        {
            OnItemRemoved?.Invoke(item, quantity - remainingToRemove);
            return remainingToRemove == 0;
        }

        return false;
    }

    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (!slot.IsEmpty() && slot.item == item)
            {
                count += slot.quantity;
            }
        }
        return count;
    }

    public bool HasItem(Item item, int quantity = 1)
    {
        return GetItemCount(item) >= quantity;
    }

    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < inventorySlots.Count)
        {
            return inventorySlots[index];
        }
        return null;
    }

    public bool SwapSlots(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= inventorySlots.Count ||
            indexB < 0 || indexB >= inventorySlots.Count)
            return false;

        InventorySlot slotA = inventorySlots[indexA];
        InventorySlot slotB = inventorySlots[indexB];

        // Store slot A data
        Item tempItem = slotA.item;
        int tempQuantity = slotA.quantity;

        // Move slot B to slot A
        slotA.item = slotB.item;
        slotA.quantity = slotB.quantity;

        // Move stored slot A data to slot B
        slotB.item = tempItem;
        slotB.quantity = tempQuantity;

        OnInventoryChanged?.Invoke(indexA);
        OnInventoryChanged?.Invoke(indexB);

        return true;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].ClearSlot();
            OnInventoryChanged?.Invoke(i);
        }
    }

    public List<InventorySlot> GetAllItems()
    {
        return inventorySlots.Where(slot => !slot.IsEmpty()).ToList();
    }

    public bool IsInventoryFull()
    {
        return inventorySlots.All(slot => !slot.IsEmpty());
    }

    public int GetEmptySlotCount()
    {
        return inventorySlots.Count(slot => slot.IsEmpty());
    }
}