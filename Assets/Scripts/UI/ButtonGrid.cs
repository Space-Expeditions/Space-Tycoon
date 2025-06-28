using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonGrid : MonoBehaviour
{
    public GameObject ReturnUI;
    public GameObject mapGrid;
    GameObject player;

    WaypointManager waypointManager;

    public List<Button> buttons = new List<Button>();

    void Start()
    {
        player = GameObject.FindFirstObjectByType<InventoryManager>().player;//GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1 && transform.GetChild(0).gameObject.activeSelf)
            {
                ReturnUI.SetActive(true);
            }
        }
    }

    public void SetWaypointButtons()
    {
        waypointManager = GameObject.FindAnyObjectByType<WaypointManager>();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = waypointManager.waypoints[i];

            if (waypointManager.waypoints[i])
                Debug.Log(i);
        }
    }

    public void AcceptReturnBtn()
    {
        waypointManager.isReturn = true;

        mapGrid.SetActive(false);
        ReturnUI.SetActive(false);

        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.transform.GetChild(0).GetComponent<GunController>().SetGunRenderer();
        player.GetComponent<PlayerMovement>().enabled = true;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.canMove = true;
        playerMovement.RetunAnimation();

        SceneManager.LoadScene("MainScene");
    }

    public void CancelBtn()
    {
        ReturnUI.SetActive(false);
    }
}