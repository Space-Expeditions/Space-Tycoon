using UnityEngine;

public class EscapeKeyManager : MonoBehaviour
{
    public GameObject seedCombinerPanel;
    public GameObject seedSelectorPanel;

    public GameObject cropPanel;
    public GameObject envPanel;
    public GameObject groundPanel;

    public SeedCombinerUI seedCombinerUI;

    public GameObject cropShopPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 씨앗 선택창 열려 있으면 그것만 닫기
            if (seedCombinerUI != null && seedCombinerUI.isSeedSelectorOpen)
            {
                seedSelectorPanel.SetActive(false);
                seedCombinerUI.isSeedSelectorOpen = false;
                return;
            }

            // 합성창 닫기
            if (seedCombinerPanel.activeSelf)
            {
                seedCombinerPanel.SetActive(false);
                return;
            }

            // crop/env/ground 창 닫기
            if (cropPanel.activeSelf || envPanel.activeSelf || groundPanel.activeSelf)
            {
                cropPanel.SetActive(false);
                envPanel.SetActive(false);
                groundPanel.SetActive(false);
                return;
            }

            if (cropShopPanel != null && cropShopPanel.activeSelf)
            {
                cropShopPanel.SetActive(false);
                return;
            }
        }
    }
}
