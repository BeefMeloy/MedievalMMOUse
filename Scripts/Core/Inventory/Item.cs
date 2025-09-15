<<<<<<< HEAD
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    public int itemID;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public ItemType itemType;

    [Header("Stack Settings")]
    public bool isStackable = true;
    public int maxStackSize = 99;

    [Header("Stats")]
    public int value;
    public float weight;

    public enum ItemType
    {
        Consumable,
        Equipment,
        Weapon,
        Armor,
        Material,
        Quest,
        Misc
    }
=======
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    public int itemID;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public ItemType itemType;

    [Header("Stack Settings")]
    public bool isStackable = true;
    public int maxStackSize = 99;

    [Header("Stats")]
    public int value;
    public float weight;

    public enum ItemType
    {
        Consumable,
        Equipment,
        Weapon,
        Armor,
        Material,
        Quest,
        Misc
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}