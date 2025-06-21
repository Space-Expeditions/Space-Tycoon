using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public InventoryPanel inventoryPanel;
    public ItemPanel toolbarPanel;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemContainer toolbarContainer;
    public ItemDragAndDropController dragAndDropController;
    public GameObject mapGrid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dragAndDropController = GameObject.FindFirstObjectByType<ItemDragAndDropController>();
    }

    public int GetItemCount(string itemName)
    {
        if (inventoryContainer != null)
            return inventoryContainer.GetItemCount(itemName);
        return 0;
    }

    public bool RemoveItem(string itemName, int count)
    {
        if (inventoryContainer != null)
            return inventoryContainer.RemoveItem(itemName, count);
        return false;
    }
}