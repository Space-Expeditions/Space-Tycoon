using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed", menuName = "Farming/SeedData")]
public class SeedData : ScriptableObject
{
    public string seedId;
    public Sprite icon;
    public string description; // 선택사항
}
