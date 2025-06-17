using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowCamera : MonoBehaviour
{
    public Transform target;      // 플레이어
    public Tilemap tilemap;       // 기준 타일맵

    private float halfWidth;
    private float halfHeight;

    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        if (target == null)
            target = GameManager.FindFirstObjectByType<GameManager>().player.transform;

        CalculateCameraBounds();
    }

    public void SetNewTilemap(Tilemap newTilemap)
    {
        tilemap = newTilemap;
        CalculateCameraBounds();
    }

    void CalculateCameraBounds()
    {
        if (tilemap == null) return;

        tilemap.CompressBounds();  // 꼭 먼저 실행

        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        Bounds bounds = tilemap.localBounds;  // 정확한 경계
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        minBounds = min + new Vector3(halfWidth, halfHeight, 0f);
        maxBounds = max - new Vector3(halfWidth, halfHeight, 0f);

    }



    void LateUpdate()
    {
        if (target == null || tilemap == null)
        {
            Debug.LogError("🚨 카메라 타겟이나 타일맵이 null입니다!");
            return;
        }

        float clampedX = Mathf.Clamp(target.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(target.position.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
