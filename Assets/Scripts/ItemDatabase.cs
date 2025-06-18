using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("��� ������ ����Ʈ")]
    public List<Item> allItems;

    private Dictionary<string, Item> itemLookup;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
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
                    Debug.LogWarning($"[ItemDatabase] �ߺ��� ������ �̸� �߰�: {item.Name}");
                }
            }
        }
    }

    public Item GetItemByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("[ItemDatabase] �� �̸����� ��û��");
            return null;
        }

        if (itemLookup == null)
        {
            Debug.LogError("[ItemDatabase] itemLookup�� �ʱ�ȭ���� ����");
            return null;
        }

        if (itemLookup.TryGetValue(name, out var item))
        {
            return item;
        }

        Debug.LogWarning($"[ItemDatabase] '{name}' �������� ã�� �� ����");
        return null;
    }
}
