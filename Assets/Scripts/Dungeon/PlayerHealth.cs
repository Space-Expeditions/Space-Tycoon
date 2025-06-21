using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    WaypointManager waypointManager;

    public int GetCurrentHealth() => currentHealth;

    void Start()
    {
        waypointManager = GameObject.FindFirstObjectByType<WaypointManager>();

        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("공격받음! 현재 체력: " + currentHealth);

        StartCoroutine(Blink());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("사망!");

        // 체력을 최대 체력의 20%로 리셋
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

}
