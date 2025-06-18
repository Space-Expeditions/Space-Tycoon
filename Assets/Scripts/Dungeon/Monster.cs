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
            Debug.LogWarning("�÷��̾ ���� �����ϴ�! �±װ� 'Player'���� Ȯ���ϼ���.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // ��ü ���� ��� (X + Y ����)
        Vector3 direction = (player.position - transform.position).normalized;

        // �̵�
        transform.position += direction * moveSpeed * Time.deltaTime;

        // ���� ���� (X ���ظ� ��)
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;  // ���� ����
        }
        else
        {
            spriteRenderer.flipX = false; // ������ ����
        }

        // ȸ�� ����
        transform.rotation = Quaternion.identity;
    }
}
