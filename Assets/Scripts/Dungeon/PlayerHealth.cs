using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    WaypointManager waypointManager;

    // 사운드 재생용
    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    public int GetCurrentHealth() => currentHealth;

    void Start()
    {
        waypointManager = GameObject.FindFirstObjectByType<WaypointManager>();

        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("공격받음! 현재 체력: " + currentHealth);

        PlaySound(hitSound);
        StartCoroutine(Blink());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("사망!");

        PlaySound(deathSound);

        currentHealth = Mathf.CeilToInt(maxHealth * 0.2f);

        waypointManager.isReturn = true;
        SceneManager.LoadScene("MainScene");
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

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
