using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CropShopUI : MonoBehaviour
{
    [Header("Reference")]
    public ItemContainer inventory;
    public List<CropPriceData> cropPrices;

    [Header("UI")]
    public GameObject cropButtonPrefab;
    public Transform cropButtonParent;
    public GameObject shopPanel;

    private void OnMouseDown()
    {
        if (!shopPanel.activeSelf)
        {
            shopPanel.SetActive(true);
            RefreshSellList();
        }
    }

    public void RefreshSellList()
    {
        foreach (Transform child in cropButtonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var priceData in cropPrices)
        {
            string cropName = priceData.cropId.Trim();
            string cropKey = cropName.ToLower();
            int ownedCount = inventory.GetItemCount(cropName);

            GameObject buttonObj = Instantiate(cropButtonPrefab, cropButtonParent);

            // 아이템 정보를 ItemDatabase에서 가져옴 (인벤토리에 없어도 가능)
            Item itemInfo = ItemDatabase.instance.GetItemByName(cropName);

            // 아이콘 설정
            Transform iconTransform = buttonObj.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && itemInfo != null)
                {
                    iconImage.sprite = itemInfo.icon;
                }
            }

            // 텍스트 설정
            Transform labelTransform = buttonObj.transform.Find("Label");
            if (labelTransform != null)
            {
                TextMeshProUGUI label = labelTransform.GetComponent<TextMeshProUGUI>();
                if (label != null)
                {
                    string displayName = itemInfo != null ? itemInfo.Name : cropName;
                    label.text = $"{displayName} ({ownedCount}개) - {priceData.CurrentPrice}G";
                }
            }

            // 클릭 이벤트: 보유 중일 때만 판매 가능
            Button button = buttonObj.GetComponent<Button>();
            if (ownedCount > 0)
            {
                string nameCopy = cropName;
                button.onClick.AddListener(() =>
                {
                    SellCrop(nameCopy, 1);
                    RefreshSellList();
                });
            }
            else
            {
                button.interactable = false; // 버튼은 비활성화하지만 표시됨
            }
        }
    }

    public void SellCrop(string cropName, int amount)
    {
        if (!inventory.RemoveItem(cropName, amount))
        {
            Debug.LogWarning($"❌ 인벤토리에 '{cropName}'이 부족합니다.");
            return;
        }

        string cropKey = cropName.Trim().ToLower();
        var priceData = cropPrices.Find(p => p.cropId.Trim().ToLower() == cropKey);

        if (priceData == null)
        {
            Debug.LogWarning($"❌ 가격 정보를 찾을 수 없습니다: {cropName}");
            return;
        }

        int totalGold = priceData.CurrentPrice * amount;
        GoldManager.Instance.AddGold(totalGold);
        Debug.Log($"✅ {cropName} 판매 완료: {totalGold}G 획득");
    }
}
