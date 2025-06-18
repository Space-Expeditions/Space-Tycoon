using UnityEngine;

public class Teleporter : MonoBehaviour
{
    Animator animator;

    WaypointManager waypointManager;
    PlayerMovement player;
    VehicleControl vehicle;

    public int num = 0;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<PlayerMovement>();
        vehicle = GameObject.FindAnyObjectByType<VehicleControl>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        waypointManager = GameObject.FindAnyObjectByType<WaypointManager>();
        
        if (animator == null || waypointManager == null)
        {
            Debug.LogWarning("필수 오브젝트가 씬에 존재하지 않습니다.");
            return;
        }

        animator.enabled = waypointManager.waypoints[num];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            float distance;
            if (!vehicle.isRiding)
                distance = Vector2.Distance(player.transform.position, transform.position);
            else
                distance = Vector2.Distance(vehicle.transform.position, transform.position);

            if (distance <= 3f)
            {
                if (!waypointManager.waypoints[num])
                {
                    waypointManager.waypoints[num] = true;
                    animator.enabled = true;
                }
            }
        }
    }

    public bool NearTeleporter()
    {
        float distance = Vector2.Distance(vehicle.transform.position, transform.position);

        if (distance <= 3f)
        {
            return true;
        }

        return false;
    }
}