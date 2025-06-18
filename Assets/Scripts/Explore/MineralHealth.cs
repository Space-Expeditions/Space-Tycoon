using UnityEngine;

public class MineralHealth : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] int dropCount = 1;
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            TakeDamage(1);
            Destroy(other.gameObject); // ���� �Ѿ� ����
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " ü��: " + currentHealth);

        StartCoroutine(Blink());

        if (currentHealth <= 0)
        {
            Die();
            DropItem();
        }
    }

    private System.Collections.IEnumerator Blink()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
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
            Debug.LogWarning("Item Prefab�� �����Ǿ� ���� �ʽ��ϴ�.");
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " ���� ���!");
        Destroy(gameObject);
    }
}