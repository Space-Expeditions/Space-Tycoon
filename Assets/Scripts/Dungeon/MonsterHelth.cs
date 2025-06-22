using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    public AudioClip hitSound; // �ǰ� �� ���� �Ҹ�
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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
        Debug.Log(gameObject.name + " ü��: " + currentHealth);

        // �ǰ� ���� ���
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

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
        Debug.Log(gameObject.name + " ���� ���!");
        Destroy(gameObject);
    }
}
