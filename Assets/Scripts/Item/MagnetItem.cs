using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float magnetRange = 1.5f;         // 자석이 작동할 범위
    public float moveSpeed = 2f;           // 끌리는 속도
    public float pickupDistance = 1f;      // 아이템 획득 거리

    Transform player;

    public Item item;
    public int count = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트가 없습니다!");
        }
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
        renderer.sortingOrder = 4; // 아이템은 항상 order 4로 표시
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(player.position.x, player.position.y)
);



        // 일정 거리 이내면 끌려감
        if (distance < magnetRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }

        // 플레이어에게 충분히 가까워지면 아이템 획득
        if (distance < pickupDistance)
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        Debug.Log($"{item.Name} 획득!");

        if (InventoryManager.instance.inventoryContainer != null)
        {
            InventoryManager.instance.inventoryContainer.Add(item, count);

            // 획득 즉시 UI 갱신 호출
            if (InventoryManager.instance.inventoryPanel != null)
                InventoryManager.instance.inventoryPanel.Show();

            if (InventoryManager.instance.toolbarPanel != null)
                InventoryManager.instance.toolbarPanel.Show();
        }
        else
        {
            Debug.LogWarning("자 잠시 소란이 있었어요");
        }

        Destroy(gameObject);
    }
}