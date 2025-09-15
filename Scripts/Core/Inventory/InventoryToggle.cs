<<<<<<< HEAD
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Button inventoryButton;

    private bool isInventoryOpen = false;

    void Start()
    {
        // Make sure inventory starts closed
        inventoryPanel.SetActive(false);

        // Add click listener to button
        inventoryButton.onClick.AddListener(ToggleInventory);
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

    void Update()
    {
        // Optional: Toggle with key press (like 'I' for inventory)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
=======
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;
    public Button inventoryButton;

    private bool isInventoryOpen = false;

    void Start()
    {
        // Make sure inventory starts closed
        inventoryPanel.SetActive(false);

        // Add click listener to button
        inventoryButton.onClick.AddListener(ToggleInventory);
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

    void Update()
    {
        // Optional: Toggle with key press (like 'I' for inventory)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}