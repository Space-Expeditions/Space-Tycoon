using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomToOutside : MonoBehaviour
{
    public Transform player;            // 🧍 플레이어
    public Transform destinationPoint;  // 📍 도달해야 하는 위치
    public Transform targetPoint;       // 🎯 이동할 위치
    public Tilemap nextTilemap;         // 🗺️ 도착한 방의 타일맵

    public float triggerRadius = 0.2f;  // 도달 판정 거리 반지름
    public float cooldown = 1f;         // 반복 이동 방지 쿨타임

    private FollowCamera camFollow;
    private float lastTeleportTime = -10f;

    void Start()
    {
        camFollow = Camera.main.GetComponent<FollowCamera>();
    }

    void Update()
    {
        if (player == null || destinationPoint == null || targetPoint == null || camFollow == null) return;

        float distance = Vector2.Distance(player.position, destinationPoint.position);

        if (distance <= triggerRadius && Time.time - lastTeleportTime >= cooldown)
        {
            Debug.Log("✅ 자동 이동 트리거 발동!");
            player.position = targetPoint.position;
            camFollow.SetNewTilemap(nextTilemap);
            lastTeleportTime = Time.time;
        }
    }
}