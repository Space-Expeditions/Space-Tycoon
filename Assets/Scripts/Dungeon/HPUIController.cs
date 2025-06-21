using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider hpSlider;

    private void Start()
    {
        if (playerHealth != null)
        {
            hpSlider.maxValue = playerHealth.maxHealth;
            hpSlider.value = playerHealth.GetCurrentHealth();
        }
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            hpSlider.value = playerHealth.GetCurrentHealth();
        }
    }
}

