using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] ItemSlot itemSlot;
    [SerializeField] GameObject itemIcon;
    RectTransform iconTransform;
    Image itemIconImage;

    // 효과음용 변수 추가
    public AudioClip dragSound;          // 드래그 시작, 드래그 중 재생용(필요하면)
    public AudioClip dropSound;          // 아이템 드롭 시 재생용
    private AudioSource audioSource;

    private bool canDrag = false;

    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconImage = itemIcon.GetComponent<Image>();

        itemIcon.SetActive(false);

        // AudioSource 컴포넌트 가져오기 (없으면 추가)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (canDrag && itemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;

            // 마우스 클릭으로 아이템 드롭 시 효과음 재생
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                    float distance = Random.Range(1.5f, 2f);
                    float angle = Random.Range(0, 2 * Mathf.PI);

                    Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
                    Vector3 spawnPos = player.position + offset;

                    ItemSpawnManager.instance.SpawnItem(spawnPos, itemSlot.item, itemSlot.count);
                    itemSlot.Clear();
                    itemIcon.SetActive(false);

                    // 드롭 사운드 재생
                    PlaySound(dropSound);
                }
            }
        }
    }

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

        // 드래그 시작 시 사운드 재생 (옵션)
        PlaySound(dragSound);
    }

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

    // 사운드 재생용 함수
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
