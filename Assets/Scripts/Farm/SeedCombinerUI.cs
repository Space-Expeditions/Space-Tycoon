using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SeedCombinerUI : MonoBehaviour
{
    [Header("UI Slots")]
    public Image slot1Image;
    public Image slot2Image;
    public Image resultImage;
    public Sprite emptySlotSprite; // ← 빈 칸용 스프라이트 (Inspector에 지정)

    [Header("UI Panels")]
    public GameObject seedCombinerPanel;
    public GameObject seedSelectorPanel;
    public Transform seedSelectionParent;

    [Header("Prefabs & Data")]
    public GameObject seedButtonPrefab; // 버튼 프리팹
    public Sprite failureSprite;
    public List<CombinationEntry> combinationTable; // 조합 테이블

    private string seed1Id = null;
    private string seed2Id = null;
    private int currentSelectingSlot = -1;

    [HideInInspector]
    public bool isSeedSelectorOpen = false;

    void OnMouseDown()
    {
        if (!seedCombinerPanel.activeSelf)
        {
            seedCombinerPanel.SetActive(true);
        }
    }

    public void OnSeedSlotClicked(int slotIndex)
    {
        currentSelectingSlot = slotIndex;
        OpenSeedSelectionPanel();
    }

    void OpenSeedSelectionPanel()
    {
        foreach (Transform child in seedSelectionParent)
        {
            Destroy(child.gameObject);
        }

        var inventory = InventoryManager.instance?.inventoryContainer;
        if (inventory == null)
        {
            Debug.LogWarning("❌ 인벤토리 연결되지 않음");
            return;
        }

        foreach (var slot in inventory.slots)
        {
            if (slot.item != null &&
                slot.item.itemType == ItemType.Seed &&
                slot.count > 0)
            {
                string id = slot.item.Name;

                // 이미 선택된 씨앗과 중복이면 스킵
                if ((currentSelectingSlot == 1 && seed2Id == id) ||
                    (currentSelectingSlot == 2 && seed1Id == id))
                {
                    continue;
                }

                GameObject btn = Instantiate(seedButtonPrefab, seedSelectionParent);
                Transform icon = btn.transform.Find("Icon");
                if (icon != null)
                {
                    icon.GetComponent<Image>().sprite = slot.item.icon;
                }

                btn.GetComponent<Button>().onClick.AddListener(() => OnSeedSelected(id));
            }
        }

        seedSelectorPanel.SetActive(true);
        isSeedSelectorOpen = true;
    }

    void OnSeedSelected(string selectedSeedId)
    {
        if (currentSelectingSlot == 1)
        {
            seed1Id = selectedSeedId;
            slot1Image.sprite = GetItemIconByName(seed1Id);
        }
        else if (currentSelectingSlot == 2)
        {
            seed2Id = selectedSeedId;
            slot2Image.sprite = GetItemIconByName(seed2Id);
        }

        currentSelectingSlot = -1;
        seedSelectorPanel.SetActive(false);
        isSeedSelectorOpen = false;

        // 씨앗 선택 시 결과창 초기화
        resultImage.sprite = emptySlotSprite;
    }

    Sprite GetItemIconByName(string id)
    {
        var item = ItemDatabase.instance.GetItemByName(id);
        return item != null ? item.icon : null;
    }

    public void CombineSeeds()
    {
        if (string.IsNullOrEmpty(seed1Id) || string.IsNullOrEmpty(seed2Id))
        {
            resultImage.sprite = failureSprite;
            return;
        }

        var ingredients = new List<string> { seed1Id, seed2Id };
        ingredients.Sort();

        string resultSeedId = null;

        foreach (var entry in combinationTable)
        {
            var sortedEntry = new List<string>(entry.ingredients);
            sortedEntry.Sort();

            if (sortedEntry.SequenceEqual(ingredients))
            {
                resultSeedId = entry.resultSeedId;
                break;
            }
        }

        if (resultSeedId != null)
        {
            resultImage.sprite = GetItemIconByName(resultSeedId);
            InventoryManager.instance.inventoryContainer.AddSeed(resultSeedId, 1);
        }
        else
        {
            resultImage.sprite = failureSprite;
        }

        InventoryManager.instance.inventoryContainer.ConsumeSeed(seed1Id);
        InventoryManager.instance.inventoryContainer.ConsumeSeed(seed2Id);

        // 슬롯 초기화 (null → emptySlotSprite)
        seed1Id = null;
        seed2Id = null;
        slot1Image.sprite = emptySlotSprite;
        slot2Image.sprite = emptySlotSprite;
    }
}
