using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 10;

    int selectedTool;
    public Action<int> onChange;

    [SerializeField] ItemContainer toolbarContainer; // 툴바의 아이템 슬롯들

    public SpriteRenderer gunSR;
    public SpriteRenderer playerSR;

    public ItemSlot selectSlot;

    private void Awake()
    {
        // GameManager에서 가져와 자동 연결
        if (toolbarContainer == null)
        {
            toolbarContainer = InventoryManager.instance.toolbarContainer;
        }
    }

    // private void Update()
    // {
    //     float delta = Input.mouseScrollDelta.y;
    //     if (delta != 0)
    //     {
    //         if (delta > 0)
    //         {
    //             selectedTool -= 1;
    //             selectedTool = (selectedTool < 0 ? toolbarSize - 1 : selectedTool);
    //         }
    //         else
    //         {
    //             selectedTool += 1;
    //             selectedTool = (selectedTool >= toolbarSize ? 0 : selectedTool);
    //         }
    //
    //         onChange?.Invoke(selectedTool);
    //
    //         selectSlot = toolbarContainer.slots[selectedTool];
    //
    //         //if (selectSlot.item != null && selectSlot.item.Name == "Gun" && playerSR.enabled)
    //         //{
    //         //    gunSR.enabled = true;
    //         //}
    //         //else
    //         //{
    //         //    gunSR.enabled = false;
    //         //}
    //     }
    //
    //     // ✅ 우클릭 감지
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         TryUseSelectedItem();
    //     }
    // }
    
    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            if (delta > 0)
            {
                selectedTool -= 1;
                selectedTool = (selectedTool < 0 ? toolbarSize - 1 : selectedTool);
            }
            else
            {
                selectedTool += 1;
                selectedTool = (selectedTool >= toolbarSize ? 0 : selectedTool);
            }

            onChange?.Invoke(selectedTool);

            selectSlot = toolbarContainer.slots[selectedTool];

            //if (selectSlot.item != null && selectSlot.item.Name == "Gun" && playerSR.enabled)
            //{
            //    gunSR.enabled = true;
            //}
            //else
            //{
            //    gunSR.enabled = false;
            //}
        }

        // ✅ 우클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // UI 위에서 클릭한 경우 처리 안 함
                return;
            }

            if (InventoryManager.instance.inventoryPanel != null &&
                !InventoryManager.instance.inventoryPanel.gameObject.activeInHierarchy)
            {
                TryUseSelectedItem();
            }
        }
    }

    internal void Set(int id)
    {
        selectedTool = id;
    }

    // ✅ 선택된 아이템 사용 시도
    private void TryUseSelectedItem()
    {
        if (toolbarContainer == null)
        {
            Debug.LogWarning("툴바 컨테이너가 연결되지 않았습니다.");
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
                // 🔒 총이 화면에 보일 때만 발사
                if (gunSR != null && gunSR.enabled)
                {
                    GunFire gun = GameObject.FindGameObjectWithTag("Player")?.GetComponentInChildren<GunFire>();
                    if (gun != null)
                    {
                        Debug.Log("빵야빵야");
                        gun.Fire();
                    }
                    else
                    {
                        Debug.LogWarning("GunFire 스크립트를 가진 오브젝트를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.Log("총이 비활성화된 상태에서는 발사되지 않습니다.");
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
}