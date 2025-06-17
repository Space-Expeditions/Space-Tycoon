using UnityEngine;

public class ClickableDrop : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;  // 드롭될 아이템 프리팹
    [SerializeField] float clickRange = 3f;  // 클릭 가능 최대 거리
    [SerializeField] int dropCount = 1;      // 드랍될 아이템 개수, 외부에서 설정 가능

    Transform player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player")?.transform;
        player = GameObject.FindFirstObjectByType<GameManager>().player?.transform;
        if (player == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트가 없습니다!");
        }
    }

    void OnMouseDown()
    {
        // 인벤토리 창 열림 여부 체크 (GameManager의 InventoryController 참고)
        if (GameManager.instance.inventoryPanel != null && GameManager.instance.inventoryPanel.gameObject.activeInHierarchy)
        {
            Debug.Log("인벤토리 창이 열려 있어서 클릭 무시");
            return; // 인벤토리 창이 열려 있으면 클릭 동작 무시
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
            Debug.Log("플레이어가 너무 멀리 있어 클릭할 수 없습니다.");
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
            Debug.LogWarning("Item Prefab이 설정되어 있지 않습니다.");
        }
    }
}

