using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] ItemSlot itemSlot;
    [SerializeField] GameObject itemIcon;
    RectTransform iconTransform;
    Image itemIconImage;

    // 드래그 가능 상태를 저장하는 플래그
    private bool canDrag = false;

    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconImage = itemIcon.GetComponent<Image>();

        // 시작 시 아이콘 비활성화
        itemIcon.SetActive(false);
    }

    private void Update()
    {
        // canDrag 가 true 이고 아이콘이 활성화된 경우에만 동작
        if (canDrag && itemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                // 마우스가 UI 위에 있지 않을 때만 로그 출력 (또는 드롭 동작)
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                    // 일정 거리 떨어진 무작위 방향으로 아이템 드랍
                    float distance = Random.Range(1.5f, 2f); // 드랍 거리 범위
                    float angle = Random.Range(0, 2 * Mathf.PI); // 무작위 방향

                    Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
                    Vector3 spawnPos = player.position + offset;

                    ItemSpawnManager.instance.SpawnItem(spawnPos, itemSlot.item, itemSlot.count);
                    itemSlot.Clear();
                    itemIcon.SetActive(false);
                }
            }
        }
    }

    // 아이템 슬롯 클릭 시 호출하는 함수
    internal void Onclick(ItemSlot slot)
    {
        if (this.itemSlot.item == null)
        {
            this.itemSlot.Copy(slot);
            slot.Clear();
        }
        else
        {
            Item item = slot.item;
            int count = slot.count;

            slot.Copy(this.itemSlot);
            this.itemSlot.Set(item, count);
        }

        UpdateIcon();
    }

    // 아이콘 활성화 상태와 canDrag 플래그를 itemSlot 상태에 맞게 동기화
    private void UpdateIcon()
    {
        if (itemSlot.item == null)
        {
            itemIcon.SetActive(false);
            canDrag = false;
        }
        else
        {
            itemIcon.SetActive(true);
            itemIconImage.sprite = itemSlot.item.icon;
            canDrag = true;
        }
    }
}
