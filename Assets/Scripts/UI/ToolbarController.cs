using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 10;
    int selectedTool;
    public Action<int> onChange;

    [SerializeField] ItemContainer toolbarContainer; // 툴바 슬롯들
    public SpriteRenderer gunSR;  // 총 스프라이트
    public SpriteRenderer playerSR;  // 플레이어 스프라이트

    public ItemSlot selectSlot;

    private void Start()
    {
        // 툴바 컨테이너가 설정되어 있는지 다시 체크
        if (toolbarContainer == null)
        {
            toolbarContainer = InventoryManager.instance.toolbarContainer;
        }

        // 초기 선택 슬롯 설정
        selectSlot = toolbarContainer.slots[selectedTool];

        // 🔁 시작할 때 총 장착 여부 반영
        if (selectSlot.item != null && selectSlot.item.Name == "Gun" && playerSR.enabled)
        {
            gunSR.enabled = true;
        }
        else
        {
            gunSR.enabled = false;
        }

        onChange?.Invoke(selectedTool); // UI 슬롯 선택 반영
    }


    private void Awake()
    {
        // GameManager에서 자동 연결
        if (toolbarContainer == null)
        {
            toolbarContainer = InventoryManager.instance.toolbarContainer;
        }
    }

    private void Update()
    {
        HandleScrollSelection();
        HandleLeftClickUse();
    }

    private void HandleScrollSelection()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            selectedTool = delta > 0
                ? (selectedTool - 1 + toolbarSize) % toolbarSize
                : (selectedTool + 1) % toolbarSize;

            onChange?.Invoke(selectedTool);

            selectSlot = toolbarContainer.slots[selectedTool];

            // 🔁 총 장착/해제 처리
            if (selectSlot.item != null && selectSlot.item.Name == "Gun" && playerSR.enabled)
            {
                gunSR.enabled = true;
            }
            else
            {
                gunSR.enabled = false;
            }
        }
    }

    private void HandleLeftClickUse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI 클릭 무시
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // 인벤토리 창이 닫혀 있을 때만 사용 허용
            if (InventoryManager.instance.inventoryPanel != null &&
                !InventoryManager.instance.inventoryPanel.gameObject.activeInHierarchy)
            {
                TryUseSelectedItem();
            }
        }
    }

    private void TryUseSelectedItem()
    {
        if (toolbarContainer == null)
        {
            Debug.LogWarning("툴바 컨테이너가 없습니다.");
            return;
        }

        if (selectedTool < 0 || selectedTool >= toolbarContainer.slots.Count)
        {
            Debug.LogWarning("선택된 툴 슬롯이 유효하지 않습니다.");
            return;
        }

        var slot = toolbarContainer.slots[selectedTool];

        if (slot.item != null)
        {
            if (slot.item.Name == "Gun")
            {
                // 🔒 총이 장착되어 있어야 발사 가능
                if (gunSR != null && gunSR.enabled)
                {
                    GunFire gun = GameObject.FindGameObjectWithTag("Player")?.GetComponentInChildren<GunFire>();
                    if (gun != null)
                    {
                        Debug.Log("빵야빵야!");
                        gun.Fire();
                    }
                    else
                    {
                        Debug.LogWarning("GunFire 스크립트를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.Log("총이 비활성화 상태입니다. 발사할 수 없습니다.");
                }
            }
            else
            {
                Debug.Log($"아이템 사용: {slot.item.Name}");
            }
        }
        else
        {
            Debug.Log("선택된 슬롯에 아이템이 없습니다.");
        }
    }

    internal void Set(int id)
    {
        selectedTool = id;
    }
}
