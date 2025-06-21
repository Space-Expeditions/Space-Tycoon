using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OxygenStatus : MonoBehaviour
{
    [SerializeField]
    Image image;
    public Sprite oxygen1;
    public Sprite oxygen2;
    public Sprite oxygen3;
    public Sprite oxygen4;
    public Sprite oxygen5;

    PlayerHealth playerHealth;

    public float currentOxygen;
    public int oxygenCount = 0;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        RefillOxygen();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            currentOxygen += Time.deltaTime;
            oxygenCount = (int)(currentOxygen / 150);
            if (oxygenCount == 1)
            {
                image.sprite = oxygen2;
            }
            else if (oxygenCount == 2)
            {
                image.sprite = oxygen3;
            }
            else if (oxygenCount == 3)
            {
                image.sprite = oxygen4;
            }
            else if (oxygenCount == 4)
            {
                image.sprite = oxygen5;
                playerHealth.Die();
            }
            else
            {
                image.sprite = oxygen1;
            }
        }
        else
        {
            if (currentOxygen != 0)
            {
                RefillOxygen();
            }
        }
    }

    public void RefillOxygen()
    {
        image.sprite = oxygen1;
        currentOxygen = 0;
        oxygenCount = 0;
    }
}
