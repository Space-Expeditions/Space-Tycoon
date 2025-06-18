using UnityEngine;

public class SeedIconManager : MonoBehaviour
{
    public GameObject potatoButton;
    public GameObject carrotButton;
    public GameObject tomatoButton;

    public ItemContainer inventory;

    void Start()
    {
        inventory = InventoryManager.instance.inventoryContainer;
    }

    public void UpdateSeedIcons()
    {
        if (inventory == null)
        {
            Debug.LogWarning("[SeedIconManager] Inventory is null");
            return;
        }

        carrotButton.SetActive(inventory.HasSeed("Carrot Seed"));
        potatoButton.SetActive(inventory.HasSeed("Potato Seed"));
        tomatoButton.SetActive(inventory.HasSeed("Tomato Seed"));
    }
}
