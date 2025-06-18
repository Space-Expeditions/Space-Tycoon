using UnityEngine;

[CreateAssetMenu(menuName = "Data/Crop Price Data")]
public class CropPriceData : ScriptableObject
{
    public string cropId;           // ex: "carrot", "potato"
    public int basePrice;
    [HideInInspector] public float currentMultiplier = 1f;

    public int CurrentPrice => Mathf.RoundToInt(basePrice * currentMultiplier);

    public void RandomizePrice()
    {
        currentMultiplier = Random.Range(0.8f, 1.5f);
    }
}

