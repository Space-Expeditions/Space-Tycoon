using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("모든 아이템 리스트")]
    public List<Item> allItems;

    private Dictionary<string, Item> itemLookup;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        BuildLookup();
    }

    private void BuildLookup()
    {
        itemLookup = new Dictionary<string, Item>();

        foreach (var item in allItems)
        {
            if (item != null)
            {
                if (!itemLookup.ContainsKey(item.Name))
                {
                    itemLookup.Add(item.Name, item);
                }
                else
                {
                    Debug.LogWarning($"[ItemDatabase] 중복된 아이템 이름 발견: {item.Name}");
                }
            }
        }
    }

    public Item GetItemByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("[ItemDatabase] 빈 이름으로 요청됨");
            return null;
        }

        if (itemLookup == null)
        {
            Debug.LogError("[ItemDatabase] itemLookup이 초기화되지 않음");
            return null;
        }

        if (itemLookup.TryGetValue(name, out var item))
        {
            return item;
        }

        Debug.LogWarning($"[ItemDatabase] '{name}' 아이템을 찾을 수 없음");
        return null;
    }
}
