using UnityEngine;

[CreateAssetMenu(menuName = "Data/Crop Price Data")]
public class CropPriceData : ScriptableObject
{
    [Tooltip("ItemDatabase�� ��ϵ� ID (ex: carrot, potato, computer1)")]
    public string cropId;

    [Tooltip("�⺻ ����")]
    public int basePrice = 10;

    [Tooltip("0.8 ~ 1.5 ���̿��� �������� �������� ���� ���")]
    [HideInInspector] public float currentMultiplier = 1f;

    // ���� �Ǹ�/���ſ� ����Ǵ� ����
    public int CurrentPrice => Mathf.RoundToInt(basePrice * currentMultiplier);

    // ȣ�� �� ������ ���� ��ȭ
    public void RandomizePrice()
    {
        currentMultiplier = Random.Range(0.8f, 1.5f);
    }

    //public string cropId;           // ex: "carrot", "potato"
    //public int basePrice;
    //[HideInInspector] public float currentMultiplier = 1f;

    //public int CurrentPrice => Mathf.RoundToInt(basePrice * currentMultiplier);

    //public void RandomizePrice()
    //{
    //    currentMultiplier = Random.Range(0.8f, 1.5f);
    //}
}

