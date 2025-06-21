using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager instance { get; private set; }
    private static List<NPCData> currentQuests;
    
    public GameObject questPanel;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questGiverText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questRequirementsText;
    public TextMeshProUGUI questRewardText;
    public TextMeshProUGUI questStateText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // 부모가 있으면 분리하여 루트 GameObject로 만듦
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (questPanel != null && questPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetMouseButtonDown(0))
            {
                HideQuestUI();
            }
        }
    }

    public static void ShowQuestsForNPC(string npcName)
    {
        if (instance != null && instance.questPanel != null)
            instance.questPanel.SetActive(true);
        currentQuests = QuestManager.instance.GetQuestsByNPC(npcName);
        // 추후 QuestAcceptUIPanel 추가 후 퀘스트 수락/거절 추가 (일단 임시로 그냥 수락 처리)
        foreach (var quest in currentQuests)
        {
            if (quest.state == QuestState.NotStarted)
            {
                QuestManager.instance.AcceptQuest(quest);
            }
        }
        foreach (var quest in currentQuests)
        {
            // 모든 필요 아이템을 모았으면 즉시 완료 및 보상 지급
            if (quest.state == QuestState.InProgress && quest.requiredItems.All(req => InventoryManager.instance.GetItemCount(req.itemName) >= req.count))
            {
                TryCompleteQuest(quest);
            }
            else
            {
                ShowQuestDetail(quest);
            }
        }
    }

    public static void ShowQuestDetail(NPCData quest)
    {
        string requirements = "조건: ";
        foreach (var req in quest.requiredItems)
        {
            req.currentCount = InventoryManager.instance.GetItemCount(req.itemName);
            requirements += $"{req.itemName} {req.currentCount}/{req.count} ";
        }
        string reward = $"보상: {quest.rewardDescription} ({quest.rewardItemName} x {quest.rewardItemCount})";
        // UI 텍스트에 데이터 매핑
        if (instance != null)
        {
            if (instance.questTitleText != null) instance.questTitleText.text = quest.questName;
            if (instance.questGiverText != null) instance.questGiverText.text = quest.npcName;
            if (instance.questDescriptionText != null) instance.questDescriptionText.text = quest.description;
            if (instance.questRequirementsText != null) instance.questRequirementsText.text = requirements;
            if (instance.questRewardText != null) instance.questRewardText.text = reward;
            if (instance.questStateText != null) instance.questStateText.text = GetQuestStateString(quest.state);
        }
    }

    public static void AcceptQuest(NPCData quest)
    {
        QuestManager.instance.AcceptQuest(quest);
        ShowQuestDetail(quest);
    }

    public static void RejectQuest(NPCData quest)
    {
        QuestManager.instance.RejectQuest(quest);
        ShowQuestDetail(quest);
    }

    public static void TryCompleteQuest(NPCData quest)
    {
        // 충족 확인
        bool allMet = true;
        foreach (var req in quest.requiredItems)
        {
            req.currentCount = InventoryManager.instance.GetItemCount(req.itemName);
            if (req.currentCount < req.count)
            {
                allMet = false;
            }
        }
        if (allMet)
        {
            // 아이템 차감
            foreach (var req in quest.requiredItems)
            {
                InventoryManager.instance.RemoveItem(req.itemName, req.count);
            }
            QuestManager.instance.CompleteQuest(quest);
            // 보상 지급
            if (!string.IsNullOrEmpty(quest.rewardItemName) && quest.rewardItemCount > 0)
            {
                var item = ItemDatabase.instance.GetItemByName(quest.rewardItemName);
                if (item != null)
                {
                    InventoryManager.instance.inventoryContainer.Add(item, quest.rewardItemCount);
                    if (InventoryManager.instance.inventoryPanel != null)
                        InventoryManager.instance.inventoryPanel.Show();
                    if (InventoryManager.instance.toolbarPanel != null)
                        InventoryManager.instance.toolbarPanel.Show();
                    Debug.Log($"보상: {quest.rewardItemName} x{quest.rewardItemCount} 지급 완료");
                }
                else
                {
                    Debug.LogWarning($"보상: {quest.rewardItemName} 아이템 데이터베이스에 없음");
                }
            }
        }
        ShowQuestDetail(quest);
    }

    public static void HideQuestUI()
    {
        if (instance != null && instance.questPanel != null)
            instance.questPanel.SetActive(false);
    }

    private static string GetQuestStateString(QuestState state)
    {
        switch (state)
        {
            case QuestState.NotStarted: return "시작 전";
            case QuestState.InProgress: return "진행 중";
            case QuestState.Completed: return "완료";
            case QuestState.Rejected: return "거절";
            case QuestState.Failed: return "실패";
            default: return state.ToString();
        }
    }
} 