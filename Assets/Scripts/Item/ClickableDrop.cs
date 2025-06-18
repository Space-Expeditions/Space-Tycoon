using UnityEngine;

public class ClickableDrop : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float clickRange = 3f;  // Ŭ�� ���� �ִ� �Ÿ�
    [SerializeField] int dropCount = 1;      // ����� ������ ����, �ܺο��� ���� ����

    Transform player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player")?.transform;
        player = GameObject.FindFirstObjectByType<InventoryManager>()?.player?.transform;
        if (player == null)
        {
            Debug.LogWarning("Player �±׸� ���� ������Ʈ�� �����ϴ�!");
        }
    }

    void OnMouseDown()
    {
        // �κ��丮 â ���� ���� üũ (GameManager�� InventoryController ����)
        if (InventoryManager.instance.inventoryPanel != null && InventoryManager.instance.inventoryPanel.gameObject.activeInHierarchy)
        {
            Debug.Log("�κ��丮 â�� ���� �־ Ŭ�� ����");
            return; // �κ��丮 â�� ���� ������ Ŭ�� ���� ����
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= clickRange)
        {
            DropItem();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("�÷��̾ �ʹ� �ָ� �־� Ŭ���� �� �����ϴ�.");
        }
    }

    public void SetDropCount(int count)
    {
        dropCount = count;
    }

    void DropItem()
    {
        if (itemPrefab != null)
        {
            for (int i = 0; i < dropCount; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.3f, 0.3f),
                    0f,
                    Random.Range(-0.3f, 0.3f)
                );

                Instantiate(itemPrefab, transform.position + randomOffset, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Item Prefab�� �����Ǿ� ���� �ʽ��ϴ�.");
        }
    }
}