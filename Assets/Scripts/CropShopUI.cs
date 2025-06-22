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
            string itemName = priceData.cropId.Trim();
            string itemKey = itemName.ToLower();
            int ownedCount = inventory.GetItemCount(itemName);

            GameObject buttonObj = Instantiate(cropButtonPrefab, cropButtonParent);
            Item itemInfo = ItemDatabase.instance.GetItemByName(itemName);

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
                    string displayName = itemInfo != null ? itemInfo.Name : itemName;
                    if (itemInfo.itemType == ItemType.Crop)
                        label.text = $"{displayName} ({ownedCount}개) - {priceData.CurrentPrice}G (판매)";
                    else if (itemInfo.itemType == ItemType.Equipment)
                        label.text = $"{displayName} - {priceData.CurrentPrice}G (구매)";
                    else
                        label.text = $"{displayName} - {priceData.CurrentPrice}G";
                }
            }

            // 버튼 이벤트 설정
            Button button = buttonObj.GetComponent<Button>();

            if (itemInfo.itemType == ItemType.Crop)
            {
                if (ownedCount > 0)
                {
                    string nameCopy = itemName;
                    button.onClick.AddListener(() =>
                    {
                        SellCrop(nameCopy, 1);
                        RefreshSellList();
                    });
                }
                else
                {
                    button.interactable = false;
                }
            }
            else if (itemInfo.itemType == ItemType.Equipment)
            {
                string nameCopy = itemName;
                button.onClick.AddListener(() =>
                {
                    TryBuyEquipment(nameCopy);
                    RefreshSellList();
                });
            }
            else
            {
                button.interactable = false;
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

    public void TryBuyEquipment(string itemName)
    {
        var item = ItemDatabase.instance.GetItemByName(itemName);
        var priceData = cropPrices.Find(p => p.cropId.Trim().ToLower() == itemName.ToLower());

        if (item == null || priceData == null)
        {
            Debug.LogWarning("❌ 잘못된 아이템 정보");
            return;
        }

        int price = priceData.CurrentPrice;

        // 안전하게 골드 차감 시도
        if (GoldManager.Instance.TrySpendGold(price))
        {
            inventory.Add(item, 1);
            Debug.Log($"🛒 {item.Name} 구매 완료!");
        }
        else
        {
            Debug.LogWarning("❌ 골드가 부족합니다.");
        }
    }
}
