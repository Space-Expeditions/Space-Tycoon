using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OxygenStatus : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    PlayerHealth playerHealth;

    public float maxOxygen = 100;
    float currentOxygen;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        RefillOxygen();
    }

    void Update()
    {
        text.text = $"Oxygen: {currentOxygen:0} / {maxOxygen}";

        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            currentOxygen -= Time.deltaTime;
            if (currentOxygen <= 0)
            {
                playerHealth.Die();
            }
        }
        else
        {
            if (currentOxygen < maxOxygen)
            {
                RefillOxygen();
            }
        }
    }

    public void RefillOxygen()
    {
        currentOxygen = maxOxygen;
    }
}
