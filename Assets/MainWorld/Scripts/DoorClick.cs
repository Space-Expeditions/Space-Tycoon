using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorClick : MonoBehaviour
{
    public Transform player;              // 🧍 플레이어
    public Transform destinationPoint;    // 🎯 이동할 위치
    public Tilemap nextTilemap;           // 🗺️ 카메라 기준 타일맵

    private FollowCamera camFollow;

    void Start()
    {
        camFollow = Camera.main.GetComponent<FollowCamera>();
    }

    void Update()
    {
        // 마우스 클릭 → 문 클릭한 경우
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                TryTeleport(); // 거리 체크 포함
            }
        }
        // 스페이스바 → 거리만 체크
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            TryTeleport(); // 거리 체크 포함
        }
    }

    void TryTeleport()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance <= 0.7f)
        {
            Debug.Log("✅ 거리 만족 → 위치 이동 시작!");
            player.position = destinationPoint.position;
            camFollow.SetNewTilemap(nextTilemap);
        }
        else
        {
            Debug.Log("❌ 너무 멀어요!");
        }
    }
}
