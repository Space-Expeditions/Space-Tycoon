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
            Debug.Log("씬 이동");
            SceneManager.LoadScene("ExploreScene");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
{
    spawnManager = GameObject.FindFirstObjectByType<PlayerSpawnManager>();
    if (spawnManager == null)
    {
        Debug.LogError("❌ PlayerSpawnManager가 활성화된 오브젝트에 없습니다.");
        return;
    }

    transform.parent.gameObject.SetActive(false);
    spawnManager.Warp(nums);
}
    }
}