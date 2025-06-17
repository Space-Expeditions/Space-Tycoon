using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MovetoDungeon : MonoBehaviour
{
    public Tilemap tilemap;

    WaypointManager waypointManager;
    FollowCamera camera;
    GameObject vehicle;

    public string targetSceneName; // 이동할 씬 이름을 유니티에서 지정 가능

    void Start()
    {
        waypointManager = GameObject.FindFirstObjectByType<WaypointManager>().GetComponent<WaypointManager>();
        camera = GameObject.FindFirstObjectByType<FollowCamera>().GetComponent<FollowCamera>();
        vehicle = GameObject.FindFirstObjectByType<VehicleControl>()?.gameObject;

        if (waypointManager.selecePointNum == -1 && !waypointManager.inDungeon)
        {
            vehicle.transform.position = transform.position + Vector3.left * 2f + Vector3.down * 2f;
            vehicle.GetComponent<VehicleControl>().Riding();
            camera.SetNewTilemap(tilemap);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                waypointManager.selecePointNum = -1;

                if (targetSceneName == "ExploreScene")
                {
                    waypointManager.inDungeon = false;
                }
                else
                {
                    waypointManager.inDungeon = true;
                }

                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogWarning("targetSceneName이 설정되어 있지 않습니다!");
            }
        }
    }
}
