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
            string cropNameKey = priceData.cropId.Trim().ToLower();
            int ownedCount = inventory.GetItemCount(priceData.cropId);

            GameObject buttonObj = Instantiate(cropButtonPrefab, cropButtonParent);

            // 인벤토리에 있는 해당 작물 슬롯 찾기
            ItemSlot slot = inventory.slots.Find(s =>
                s.item != null &&
                s.item.itemType == ItemType.Crop &&
                s.item.Name.Trim().ToLower() == cropNameKey
            );

            // 아이콘 설정
            Transform iconTransform = buttonObj.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = slot?.item?.icon;
                }
            }

            // 텍스트 설정
            Transform labelTransform = buttonObj.transform.Find("Label");
            if (labelTransform != null)
            {
                TextMeshProUGUI label = labelTransform.GetComponent<TextMeshProUGUI>();
                if (label != null)
                {
                    string displayName = slot?.item?.Name ?? priceData.cropId;
                    label.text = $"{displayName} ({ownedCount}개) - {priceData.CurrentPrice}G";
                }
            }

            // 클릭 이벤트: 보유 중인 작물만 가능
            if (ownedCount > 0)
            {
                string nameCopy = priceData.cropId;
                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SellCrop(nameCopy, 1);
                    RefreshSellList();
                });
            }
            else
            {
                buttonObj.GetComponent<Button>().interactable = false;
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

        string cropNameKey = cropName.Trim().ToLower();
        var priceData = cropPrices.Find(p => p.cropId.Trim().ToLower() == cropNameKey);

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
