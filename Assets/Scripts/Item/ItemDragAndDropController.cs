using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] ItemSlot itemSlot;
    [SerializeField] GameObject itemIcon;
    RectTransform iconTransform;
    Image itemIconImage;

    // �巡�� ���� ���¸� �����ϴ� �÷���
    private bool canDrag = false;

    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconImage = itemIcon.GetComponent<Image>();

        // ���� �� ������ ��Ȱ��ȭ
        itemIcon.SetActive(false);
    }

    private void Update()
    {
        // canDrag �� true �̰� �������� Ȱ��ȭ�� ��쿡�� ����
        if (canDrag && itemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                // ���콺�� UI ���� ���� ���� ���� �α� ��� (�Ǵ� ��� ����)
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                    // ���� �Ÿ� ������ ������ �������� ������ ���
                    float distance = Random.Range(1.5f, 2f); // ��� �Ÿ� ����
                    float angle = Random.Range(0, 2 * Mathf.PI); // ������ ����

                    Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
                    Vector3 spawnPos = player.position + offset;

                    ItemSpawnManager.instance.SpawnItem(spawnPos, itemSlot.item, itemSlot.count);
                    itemSlot.Clear();
                    itemIcon.SetActive(false);
                }
            }
        }
    }

    // ������ ���� Ŭ�� �� ȣ���ϴ� �Լ�
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

    // ������ Ȱ��ȭ ���¿� canDrag �÷��׸� itemSlot ���¿� �°� ����ȭ
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