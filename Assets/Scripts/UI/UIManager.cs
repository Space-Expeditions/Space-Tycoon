using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static List<UIManager> instances = new List<UIManager>();
    public static List<UIManager> Instances => instances;

    [Header("UI Panels")]
    [SerializeField] private GameObject miniMapPanel;
    [SerializeField] private GameObject toolbarPanel;
    [SerializeField] private GameObject inventorPanel;

    [Header("Player Scripts")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ToolbarController toolbarController;
    [SerializeField] private InventoryController inventoryController;

    private List<GameObject> activePanels = new List<GameObject>();
    private bool wasAnyPanelActive = false;

    private void Awake()
    {
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);
        
        instances.Add(this);
        
        if (playerMovement != null) playerMovement.enabled = true;
        if (toolbarController != null) toolbarController.enabled = true;
        if (inventoryController != null) inventoryController.enabled = true;
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }

    private void Update()
    {
        bool hasPanelChanged = false;
        activePanels.Clear();

        foreach (var uiManager in instances)
        {
            if (uiManager != null)
            {
                foreach (Transform child in uiManager.transform)
                {
                    if (child.gameObject.activeSelf && 
                        child.gameObject != uiManager.miniMapPanel && 
                        child.gameObject != uiManager.toolbarPanel)
                    {
                        activePanels.Add(child.gameObject);
                        hasPanelChanged = true;
                    }
                }
            }
        }
        
        if (hasPanelChanged || wasAnyPanelActive != (activePanels.Count > 0))
        {
            bool anyPanelActive = activePanels.Count > 0;
            
            foreach (var uiManager in instances)
            {
                if (uiManager != null)
                {
                    if (uiManager.playerMovement != null) uiManager.playerMovement.enabled = !anyPanelActive;
                    if (uiManager.toolbarController != null) uiManager.toolbarController.enabled = !anyPanelActive;
                    
                    if (uiManager.inventoryController != null && !uiManager.inventorPanel.activeInHierarchy) 
                        uiManager.inventoryController.enabled = !anyPanelActive;
                    if (uiManager.toolbarPanel != null) uiManager.toolbarPanel.SetActive(!anyPanelActive);
                    if (uiManager.miniMapPanel != null) uiManager.miniMapPanel.SetActive(!anyPanelActive);
                }
            }

            wasAnyPanelActive = anyPanelActive;
        }
    }

    public static List<UIManager> GetAllInstances()
    {
        return new List<UIManager>(instances);
    }

    public static UIManager GetFirstInstance()
    {
        return instances.Count > 0 ? instances[0] : null;
    }

    public static UIManager GetInstanceByName(string name)
    {
        return instances.Find(ui => ui.name == name);
    }

    public static List<GameObject> GetAllUIPanels()
    {
        List<GameObject> allPanels = new List<GameObject>();
        foreach (var uiManager in instances)
        {
            if (uiManager != null)
            {
                foreach (Transform child in uiManager.transform)
                {
                    allPanels.Add(child.gameObject);
                }
            }
        }
        return allPanels;
    }

    public static List<T> GetUIPanelsByType<T>() where T : Component
    {
        List<T> panels = new List<T>();
        foreach (var uiManager in instances)
        {
            if (uiManager != null)
            {
                T[] components = uiManager.GetComponentsInChildren<T>();
                panels.AddRange(components);
            }
        }
        return panels;
    }
}