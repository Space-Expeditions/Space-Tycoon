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

    private void Awake()
    {
        if (toolbarContainer == null)
        {
            toolbarContainer = InventoryManager.instance.toolbarContainer;
        }
    }

    private void Start()
    {
        if (toolbarContainer == null)
        {
            toolbarContainer = InventoryManager.instance.toolbarContainer;
        }

        selectedTool = Mathf.Clamp(selectedTool, 0, toolbarSize - 1);
        selectSlot = toolbarContainer.slots[selectedTool];

        UpdateGunEquipState(); // 시작할 때 총 반영

        onChange?.Invoke(selectedTool);
    }

    private void Update()
    {
        HandleScrollSelection();
        HandleLeftClickUse();
        UpdateGunEquipState(); // 매 프레임 슬롯 상태 체크 (드랍 반영)
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
        }
    }

    private void HandleLeftClickUse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

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

    // ✅ 드랍 반영 포함: 아이템 상태에 따라 총 스프라이트 켜고 끄기
    private void UpdateGunEquipState()
    {
        if (selectSlot != null && selectSlot.item != null &&
            selectSlot.item.Name == "Gun" && playerSR.enabled)
        {
            gunSR.enabled = true;
        }
        else
        {
            gunSR.enabled = false;
        }
    }

    internal void Set(int id)
    {
        selectedTool = Mathf.Clamp(id, 0, toolbarSize - 1);
        selectSlot = toolbarContainer.slots[selectedTool];
        UpdateGunEquipState(); // 직접 슬롯 설정 시에도 총 상태 반영
    }
}
