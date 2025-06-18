using UnityEngine;
using System.Collections.Generic;

public class CropPriceManager : MonoBehaviour
{
    public List<CropPriceData> cropPrices;
    public float priceUpdateInterval = 60f;

    private void Start()
    {
        UpdateAllPrices();
        InvokeRepeating(nameof(UpdateAllPrices), priceUpdateInterval, priceUpdateInterval);
    }

    void UpdateAllPrices()
    {
        foreach (var priceData in cropPrices)
        {
            priceData.RandomizePrice();
        }

        Debug.Log("📉 작물 가격 갱신 완료");
    }
}
