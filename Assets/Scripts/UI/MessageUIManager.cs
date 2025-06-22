using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class MessageUIManager : MonoBehaviour
{
    public static MessageUIManager instance { get; private set; }
    public GameObject messagePanel;
    public Text titleText;
    public Text contentText;

    private List<string> scenarioMessages = new List<string>();
    private int currentMessageIndex = 0;
    private bool isActive = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowMessage(string npcName)
    {
        if (!NPCData.npcScenarios.ContainsKey(npcName))
        {
            scenarioMessages = new List<string> { "..." };
        }
        else
        {
            // 시나리오 중 랜덤으로 하나만 선택
            var all = NPCData.npcScenarios[npcName];
            int rand = Random.Range(0, all.Count);
            scenarioMessages = new List<string> { all[rand] };
        }
        currentMessageIndex = 0;
        isActive = true;
        messagePanel.SetActive(true);
        titleText.text = npcName;
        contentText.text = scenarioMessages[currentMessageIndex];
    }

    private void Update()
    {
        if (!isActive || !messagePanel.activeSelf) return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Backspace))
        {
            messagePanel.SetActive(false);
            isActive = false;
        }
    }

    public static void TryCompleteQuestForNPC(string npcName)
    {
        if (QuestManager.instance == null || QuestUIManager.instance == null) return;
        var quests = QuestManager.instance.GetQuestsByNPC(npcName);
        foreach (var quest in quests)
        {
            if (quest.state == QuestState.InProgress && quest.requiredItems.All(req => InventoryManager.instance.GetItemCount(req.itemName) >= req.count))
            {
                QuestUIManager.TryCompleteQuest(quest);
            }
        }
    }
}