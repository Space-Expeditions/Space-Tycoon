using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    // TODO: Instance
    // private static UIManager instance;
    // public static UIManager Instance => instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject miniMapPanel;
    [SerializeField] private GameObject toolbarPanel;
    [SerializeField] private GameObject inventorPanel;
    [SerializeField] private GameObject oxygenPanel;
    [SerializeField] private GameObject hpBarPanel;
    [SerializeField] private GameObject uiMessagePanel;
    [SerializeField] private GameObject questUIPanel;
    // [SerializeField] private GameObject player;

    [Header("Player Scripts")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ToolbarController toolbarController;
    [SerializeField] private InventoryController inventoryController;

    private List<GameObject> activePanels = new List<GameObject>();
    private bool wasAnyPanelActive = false;

    private void Awake()
    {
        // if (instance == null)
        // {
        //     instance = this;
                DontDestroyOnLoad(gameObject);
        //     
        //     if (player != null)
        //     {
                if (playerMovement != null) playerMovement.enabled = true;
                if (toolbarController != null) toolbarController.enabled = true;
                if (inventoryController != null) inventoryController.enabled = true;
        //     }
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    private void Update()
    {
        bool hasPanelChanged = false;
        activePanels.Clear();

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf && 
                child.gameObject != miniMapPanel && 
                child.gameObject != toolbarPanel &&
                child.gameObject != oxygenPanel &&
                child.gameObject != hpBarPanel &&
                child.gameObject != uiMessagePanel &&
                child.gameObject != questUIPanel)
            {
                activePanels.Add(child.gameObject);
                hasPanelChanged = true;
            }
        }
        
        if (hasPanelChanged || wasAnyPanelActive != (activePanels.Count > 0))
        {
            bool anyPanelActive = activePanels.Count > 0;
            
            if (playerMovement != null) playerMovement.enabled = !anyPanelActive;
            if (toolbarController != null) toolbarController.enabled = !anyPanelActive;
            if (inventoryController != null && !inventorPanel.activeInHierarchy) inventoryController.enabled = !anyPanelActive;
            if (toolbarPanel != null) toolbarPanel.SetActive(!anyPanelActive);
            if (hpBarPanel != null) hpBarPanel.SetActive(!anyPanelActive);
            if (oxygenPanel != null) oxygenPanel.SetActive(!anyPanelActive);
            if (miniMapPanel != null) miniMapPanel.SetActive(!anyPanelActive);

            wasAnyPanelActive = anyPanelActive;
        }
    }
}