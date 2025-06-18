using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}

[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;

    public void Add(Item item, int count = 1)
    {
        if (item.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == item);
            if (itemSlot != null)
            {
                itemSlot.count += count;
            }
            else
            {
                itemSlot = slots.Find(x => x.item == null);
                if (itemSlot != null)
                {
                    itemSlot.Set(item, count);
                }
            }
        }
        else
        {
            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if (itemSlot != null)
            {
                itemSlot.item = item;
                itemSlot.count = 1;
            }
        }
    }

    public bool HasSeed(string seedName)
    {
        seedName = seedName.Trim().ToLower();

        foreach (var slot in slots)
        {
            if (slot.item != null &&
                slot.item.itemType == ItemType.Seed &&
                slot.item.Name.Trim().ToLower() == seedName &&
                slot.count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void ConsumeSeed(string seedName, int amount = 1)
    {
        seedName = seedName.Trim().ToLower();

        foreach (var slot in slots)
        {
            if (slot.item != null &&
                slot.item.itemType == ItemType.Seed &&
                slot.item.Name.Trim().ToLower() == seedName &&
                slot.count >= amount)
            {
                slot.count -= amount;
                if (slot.count <= 0)
                {
                    slot.Clear();
                }

                Debug.Log($"✅ 씨앗 '{seedName}' 사용 완료. 남은 개수: {slot.count}");
                return;
            }
        }

        Debug.LogWarning($"❌ 씨앗 '{seedName}' 인벤토리에서 찾지 못했거나 개수가 부족합니다.");
    }

    public void AddSeed(string seedName, int count = 1)
    {
        Item seedItem = FindSeedByName(seedName);
        if (seedItem != null)
        {
            Add(seedItem, count);
        }
        else
        {
            Debug.LogWarning($"[ItemContainer] Seed item '{seedName}' not found.");
        }
    }

    public int GetItemCount(string itemName)
    {
        string nameKey = itemName.Trim().ToLower();
        var slot = slots.Find(s => s.item != null && s.item.Name.Trim().ToLower() == nameKey);
        return slot != null ? slot.count : 0;
    }

    public bool RemoveItem(string itemName, int count)
    {
        string nameKey = itemName.Trim().ToLower();
        var slot = slots.Find(s => s.item != null && s.item.Name.Trim().ToLower() == nameKey);
        if (slot != null && slot.count >= count)
        {
            slot.count -= count;
            if (slot.count <= 0)
            {
                slot.Clear();
            }
            return true;
        }
        return false;
    }

    private Item FindSeedByName(string seedName)
    {
        return ItemDatabase.instance?.GetItemByName(seedName);
    }
}
