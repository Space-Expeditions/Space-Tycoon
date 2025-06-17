using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MoveToDungeon : MonoBehaviour
{
    public Tilemap tilemap;

    WaypointManager waypointManager;
    FollowCamera camera;
    GameObject vehicle;

    public string targetSceneName; // �̵��� �� �̸��� ����Ƽ���� ���� ����

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
                Debug.LogWarning("targetSceneName�� �����Ǿ� ���� �ʽ��ϴ�!");
            }
        }
    }
}