using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform player;
    public float offsetDistance = 0.5f;
    private SpriteRenderer gunRenderer;

    void Start()
    {
        gunRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - player.position).normalized;

        // 총 위치 갱신
        transform.position = player.position + direction * offsetDistance;

        // 총 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 🔁 총 좌우 반전 처리
        if (direction.x < 0)
        {
            gunRenderer.flipY = true; // 왼쪽 보면 뒤집음
        }
        else
        {
            gunRenderer.flipY = false;
        }

        // 🔽 총이 플레이어 앞/뒤에 보이도록 처리
        // if (direction.y > 0)
        // {
        //     gunRenderer.sortingOrder = -1; // 뒤로 감
        // }
        // else
        // {
        //     gunRenderer.sortingOrder = 1; // 앞으로 나옴
        // }
    }
}
