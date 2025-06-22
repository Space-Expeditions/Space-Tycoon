using UnityEngine;
using UnityEngine.SceneManagement;

public class ExploreButton : MonoBehaviour
{
    WaypointManager waypointManager;
    PlayerSpawnManager spawnManager;

    public int nums = 0;

    public AudioClip clickSound;  // 인스펙터에서 넣을 효과음
    private AudioSource audioSource;

    void Start()
    {
        waypointManager = GameObject.FindAnyObjectByType<WaypointManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Explore()
    {
        // 버튼 클릭 시 효과음 재생
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);

        waypointManager.selecePointNum = nums;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerMovement playerMovement = PlayerMovement.FindAnyObjectByType<PlayerMovement>();
            playerMovement.RetunAnimation();
            Debug.Log("씬 이동");
            SceneManager.LoadScene(1);
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
