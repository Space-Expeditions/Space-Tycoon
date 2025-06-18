using UnityEngine;

public enum ItemType
{
    Seed,
    Tool,
    Crop,
    Etc
}

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public ItemType itemType; // ✅ 추가된 필드
    public bool stackable;
    public Sprite icon;
}