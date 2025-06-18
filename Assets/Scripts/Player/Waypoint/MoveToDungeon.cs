using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MoveToDungeon : MonoBehaviour
{
    new FollowCamera camera;
    
    WaypointManager waypointManager;
    GameObject vehicle;
    
    public Tilemap tilemap;
    
    public string targetSceneName;

    void Start()
    {
        waypointManager = GameObject.FindFirstObjectByType<WaypointManager>();
        camera = GameObject.FindFirstObjectByType<FollowCamera>();
        vehicle = GameObject.FindFirstObjectByType<VehicleControl>()?.gameObject;

        if (waypointManager == null)
            Debug.LogWarning("❌ WaypointManager 오브젝트를 찾을 수 없습니다.");

        if (camera == null)
            Debug.LogWarning("❌ FollowCamera 오브젝트를 찾을 수 없습니다.");

        if (vehicle == null)
            Debug.LogWarning("❌ VehicleControl 오브젝트를 찾을 수 없습니다.");

        if (waypointManager == null || camera == null || vehicle == null)
        {
            Debug.LogWarning("⚠️ 필수 오브젝트가 하나 이상 씬에 존재하지 않습니다. 초기화 중단.");
            return;
        }
        
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
                Debug.LogWarning("targetSceneName�� �����Ǿ� ���� �ʽ��ϴ�!");
            }
        }
    }
}