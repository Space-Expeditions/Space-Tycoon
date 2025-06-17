using UnityEngine;
using UnityEngine.SceneManagement;

public class ExploreButton : MonoBehaviour
{
    WaypointManager waypointManager;
    PlayerSpawnManager spawnManager;

    public int nums = 0;

    void Start()
    {
        waypointManager = GameObject.FindAnyObjectByType<WaypointManager>();
    }

    public void Explore()
    {
        waypointManager.selecePointNum = nums;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerMovement playerMovement = PlayerMovement.FindAnyObjectByType<PlayerMovement>();
            playerMovement.RetunAnimation();

            SceneManager.LoadScene(1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            spawnManager = GameObject.FindFirstObjectByType<PlayerSpawnManager>();

            transform.parent.gameObject.SetActive(false);

            spawnManager.Warp(nums);
        }
    }
}