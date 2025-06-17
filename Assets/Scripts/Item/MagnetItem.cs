using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float magnetRange = 1.5f;         // �ڼ��� �۵��� ����
    public float moveSpeed = 2f;           // ������ �ӵ�
    public float pickupDistance = 1f;      // ������ ȹ�� �Ÿ�

    Transform player;

    public Item item;
    public int count = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� �����ϴ�!");
        }
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ���� �Ÿ� �̳��� ������
        if (distance < magnetRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }

        // �÷��̾�� ����� ��������� ������ ȹ��
        if (distance < pickupDistance)
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        Debug.Log($"{item.Name} ȹ��!");

        if (InventoryManager.instance.inventoryContainer != null)
        {
            InventoryManager.instance.inventoryContainer.Add(item, count);

            // ȹ�� ��� UI ���� ȣ��
            if (InventoryManager.instance.inventoryPanel != null)
                InventoryManager.instance.inventoryPanel.Show();

            if (InventoryManager.instance.toolbarPanel != null)
                InventoryManager.instance.toolbarPanel.Show();
        }
        else
        {
            Debug.LogWarning("�� ��� �Ҷ��� �־����");
        }

        Destroy(gameObject);
    }
}