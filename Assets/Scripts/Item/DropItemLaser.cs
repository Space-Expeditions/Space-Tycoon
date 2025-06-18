using UnityEngine;

public class DropItemLaser : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] int dropCount = 1;

    public void SetDropCount(int count)
    {
        dropCount = count;
    }

    void DropItem()
    {
        if (itemPrefab != null)
        {
            for (int i = 0; i < dropCount; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.3f, 0.3f),
                    0f,
                    Random.Range(-0.3f, 0.3f)
                );

                Instantiate(itemPrefab, transform.position + randomOffset, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Item Prefab이 설정되어 있지 않습니다.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))  // 레이저 태그 확인
        {
            DropItem();
            Destroy(gameObject);     // 자신 제거
            Destroy(collision.gameObject);  // 레이저도 제거
        }
    }
}

