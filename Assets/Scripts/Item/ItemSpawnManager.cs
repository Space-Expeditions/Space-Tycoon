using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject pickUpItemPrefab;

    public GameObject PickUpItemPrefab
    {
        get => pickUpItemPrefab;
        set => pickUpItemPrefab = value;
    }

    public void SpawnItem(Vector3 position, Item item, int count)
    {
        if (pickUpItemPrefab == null)
        {
            Debug.LogError("pickUpItemPrefab�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }
        GameObject o = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        o.transform.localScale = Vector3.one * 5f;
        o.GetComponent<MagnetItem>().Set(item, count);
    }
}
