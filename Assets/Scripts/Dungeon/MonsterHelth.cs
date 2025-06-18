using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
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
            Destroy(other.gameObject); // 맞은 총알 삭제
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " 체력: " + currentHealth);

        StartCoroutine(Blink());

        if (currentHealth <= 0)
        {
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

    void Die()
    {
        Debug.Log(gameObject.name + " 몬스터 사망!");
        Destroy(gameObject);
    }
}
