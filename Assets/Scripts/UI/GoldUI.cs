using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText; // Inspector¿¡ ¿¬°á

    void Start()
    {
        UpdateGoldUI(GoldManager.Instance.Gold);
        GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
    }

    void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= UpdateGoldUI;
    }

    void UpdateGoldUI(int newGold)
    {
        goldText.text = $"{newGold} G";
    }
}
