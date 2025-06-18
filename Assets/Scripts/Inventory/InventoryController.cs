using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject toolbarPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
            toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
        }
    }
    public bool IsOpen()
    {
        return inventoryPanel.activeInHierarchy;
    }
}