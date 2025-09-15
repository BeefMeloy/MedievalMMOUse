<<<<<<< HEAD
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }

    public InventorySlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }

    public bool CanAddItem(Item itemToAdd, int quantityToAdd)
    {
        if (IsEmpty())
            return true;

        if (item == itemToAdd && item.isStackable)
        {
            return quantity + quantityToAdd <= item.maxStackSize;
        }

        return false;
    }

    public void AddItem(Item itemToAdd, int quantityToAdd)
    {
        if (IsEmpty())
        {
            item = itemToAdd;
            quantity = quantityToAdd;
        }
        else if (item == itemToAdd && item.isStackable)
        {
            quantity += quantityToAdd;
        }
    }

    public void RemoveItem(int quantityToRemove)
    {
        quantity -= quantityToRemove;
        if (quantity <= 0)
        {
            item = null;
            quantity = 0;
        }
    }

    public void ClearSlot()
    {
        item = null;
        quantity = 0;
    }

    // ADD THIS NEW METHOD for UI updates
    public void SetSlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
=======
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }

    public InventorySlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }

    public bool CanAddItem(Item itemToAdd, int quantityToAdd)
    {
        if (IsEmpty())
            return true;

        if (item == itemToAdd && item.isStackable)
        {
            return quantity + quantityToAdd <= item.maxStackSize;
        }

        return false;
    }

    public void AddItem(Item itemToAdd, int quantityToAdd)
    {
        if (IsEmpty())
        {
            item = itemToAdd;
            quantity = quantityToAdd;
        }
        else if (item == itemToAdd && item.isStackable)
        {
            quantity += quantityToAdd;
        }
    }

    public void RemoveItem(int quantityToRemove)
    {
        quantity -= quantityToRemove;
        if (quantity <= 0)
        {
            item = null;
            quantity = 0;
        }
    }

    public void ClearSlot()
    {
        item = null;
        quantity = 0;
    }

    // ADD THIS NEW METHOD for UI updates
    public void SetSlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}