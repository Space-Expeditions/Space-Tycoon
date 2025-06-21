using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    public PlayerHealth playerHealth;  // 플레이어 체력 스크립트 참조
    public Slider hpSlider;             // UI 슬라이더 참조

    void Start()
    {
        if (playerHealth != null && hpSlider != null)
        {
            hpSlider.maxValue = playerHealth.maxHealth;   // 최대 체력으로 maxValue 설정
            hpSlider.value = playerHealth.GetCurrentHealth();  // 현재 체력으로 초기값 설정
        }
    }

    void Update()
    {
        if (playerHealth != null && hpSlider != null)
        {
            hpSlider.value = playerHealth.GetCurrentHealth();  // 매 프레임마다 현재 체력에 맞춰 슬라이더 갱신
        }
    }
}


