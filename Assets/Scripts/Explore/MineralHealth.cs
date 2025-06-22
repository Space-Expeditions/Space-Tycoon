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
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        StartCoroutine(Blink());

        if (currentHealth <= 0)
        {
            DropItem();
            Die();
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
                Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.3f, 0.3f));
                Instantiate(itemPrefab, transform.position + offset, Quaternion.identity);
            }
        }
    }

    void Die()
    {
        // ✅ 여기서 소리 재생
        if (CompareTag("Rock") && MineralSoundManager.instance != null)
        {
            MineralSoundManager.instance.PlayBreakSound(transform.position);
        }

        Destroy(gameObject);
    }
}
