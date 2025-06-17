using UnityEngine;

public class NearVehicle : MonoBehaviour
{
    public GameObject mapGrid;

    PlayerMovement player;
    ButtonGrid buttonGrid;
    WaypointManager waypointManager;

    float animTime = 1;
    float currentAnimTime = 0;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>();
        buttonGrid = GameObject.FindAnyObjectByType<ButtonGrid>();
        waypointManager = GameObject.FindFirstObjectByType<WaypointManager>();
        mapGrid = InventoryManager.FindFirstObjectByType<InventoryManager>().mapGrid;

        if (waypointManager.isReturn)
        {
            player.transform.position = transform.GetChild(0).transform.position;
        }

        currentAnimTime = animTime;
    }

    bool playAnim = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);
            if (distance <= 2f)
            {
                Debug.Log("Near to vehicle");

                playAnim = true;
            }
            else
            {
                Debug.Log("Far to vehicle");
            }
        }

        if (playAnim)
        {
            if (!mapGrid.activeSelf)
                player.CallAnimation("Device");
            else
                player.CallAnimation("ReverseDevice");

            currentAnimTime -= Time.deltaTime;
            if (currentAnimTime <= 0f)
            {
                if (!mapGrid.activeSelf)
                {
                    mapGrid.SetActive(true);
                    buttonGrid.SetWaypointButtons();

                    player.animCheck = false;
                }
                else
                {
                    mapGrid.SetActive(false);

                    player.RetunAnimation();
                    player.canMove = true;
                }

                playAnim = false;
                currentAnimTime = animTime;
            }
        }
    }
}