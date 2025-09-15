using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Gameplay/Character Model")]
public class CharacterModel : MonoBehaviour
{
    [Header("Equipment Slots")]
    [SerializeField] private Transform headSlot;
    [SerializeField] private Transform chestSlot;
    [SerializeField] private Transform legsSlot;
    [SerializeField] private Transform feetSlot;
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform shieldSlot;

    [Header("Base Model")]
    [SerializeField] private GameObject baseModel;

    private Dictionary<EquipmentSlot, GameObject> equippedItems = new Dictionary<EquipmentSlot, GameObject>();

    public void EquipItem(EquipmentSlot slot, GameObject itemPrefab)
    {
        // Remove existing item
        UnequipItem(slot);

        if (!itemPrefab) return;

        Transform targetSlot = GetSlotTransform(slot);
        if (!targetSlot) return;

        // Instantiate and attach
        GameObject newItem = Instantiate(itemPrefab, targetSlot);
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

        equippedItems[slot] = newItem;

        // Update any portrait renderers
        NotifyPortraitUpdate();
    }

    public void UnequipItem(EquipmentSlot slot)
    {
        if (equippedItems.TryGetValue(slot, out GameObject item))
        {
            if (item) DestroyImmediate(item);
            equippedItems.Remove(slot);
            NotifyPortraitUpdate();
        }
    }

    private Transform GetSlotTransform(EquipmentSlot slot)
    {
        return slot switch
        {
            EquipmentSlot.Head => headSlot,
            EquipmentSlot.Chest => chestSlot,
            EquipmentSlot.Legs => legsSlot,
            EquipmentSlot.Feet => feetSlot,
            EquipmentSlot.Weapon => weaponSlot,
            EquipmentSlot.Shield => shieldSlot,
            _ => null
        };
    }

    private void NotifyPortraitUpdate()
    {
        // Clear portrait cache when equipment changes
        var portraitRenderer = FindFirstObjectByType<CharacterPortraitRenderer>();
        if (portraitRenderer)
        {
            portraitRenderer.ClearCache();
        }
    }
}

public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Weapon,
    Shield
}