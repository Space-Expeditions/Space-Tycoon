using UnityEngine;

public class MonsterFollow : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("플레이어가 씬에 없습니다! 태그가 'Player'인지 확인하세요.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // 전체 방향 계산 (X + Y 포함)
        Vector3 direction = (player.position - transform.position).normalized;

        // 이동
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 방향 반전 (X 기준만 비교)
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;  // 왼쪽 보기
        }
        else
        {
            spriteRenderer.flipX = false; // 오른쪽 보기
        }

        // 회전 방지
        transform.rotation = Quaternion.identity;
    }
}
