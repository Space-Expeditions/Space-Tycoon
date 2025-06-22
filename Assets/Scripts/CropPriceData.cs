using UnityEngine;

[CreateAssetMenu(menuName = "Data/Crop Price Data")]
public class CropPriceData : ScriptableObject
{
    [Tooltip("ItemDatabase에 등록된 ID (ex: carrot, potato, computer1)")]
    public string cropId;

    [Tooltip("기본 가격")]
    public int basePrice = 10;

    [Tooltip("0.8 ~ 1.5 사이에서 랜덤으로 곱해지는 가격 배수")]
    [HideInInspector] public float currentMultiplier = 1f;

    // 실제 판매/구매에 적용되는 가격
    public int CurrentPrice => Mathf.RoundToInt(basePrice * currentMultiplier);

    // 호출 시 랜덤한 가격 변화
    public void RandomizePrice()
    {
        currentMultiplier = Random.Range(0.8f, 1.5f);
    }
}
