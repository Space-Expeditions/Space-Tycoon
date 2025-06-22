using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float magnetRange = 1.5f;
    public float moveSpeed = 2f;
    public float pickupDistance = 1f;

    Transform player;

    public Item item;
    public int count = 1;

    // 효과음용 변수 추가
    public AudioClip pickupSound;
    public float pickupSoundVolume = 1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다!");
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

        if (distance < magnetRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }

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

            if (InventoryManager.instance.inventoryPanel != null)
                InventoryManager.instance.inventoryPanel.Show();

            if (InventoryManager.instance.toolbarPanel != null)
                InventoryManager.instance.toolbarPanel.Show();
        }
        else
        {
            Debug.LogWarning("인벤토리 컨테이너가 없습니다");
        }

        // 효과음 재생 (현재 위치에서)
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);
        }

        Destroy(gameObject);
    }
}
