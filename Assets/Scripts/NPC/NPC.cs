using UnityEngine;
using System.Linq;

public class NPC : MonoBehaviour
{
    public string npcName;
    public float interactDistance = 2f;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.tag = "NPC";
    }

    private void Update()
    {
        if (player == null) return;
        float dist = Vector2.Distance(player.transform.position, transform.position);
        bool nowNear = dist <= interactDistance;

        // 대화 호출
        if (nowNear && Input.GetKeyDown(KeyCode.T) && (QuestUIManager.instance == null || !QuestUIManager.instance.questPanel.activeSelf))
        {
            MessageUIManager.instance.ShowMessage(npcName);
        }

        // 퀘스트 호출
        if (nowNear && Input.GetKeyDown(KeyCode.Q) && (MessageUIManager.instance == null || !MessageUIManager.instance.messagePanel.activeSelf))
        {
            if (!string.IsNullOrEmpty(npcName) && QuestManager.instance != null && QuestUIManager.instance != null)
            {
                var quests = QuestManager.instance.GetQuestsByNPC(npcName);
                bool completedAny = false;
                foreach (var quest in quests.Where(q => q.state == QuestState.InProgress))
                {
                    bool allMet = quest.requiredItems.All(req =>
                        InventoryManager.instance.GetItemCount(req.itemName) >= req.count);
                    if (allMet)
                    {
                        QuestUIManager.TryCompleteQuest(quest);
                        completedAny = true;
                    }
                }

                if (!completedAny)
                {
                    QuestUIManager.ShowQuestsForNPC(npcName);
                }
            }
            else
            {
                Debug.LogWarning("필수 오브젝트가 씬에 존재하지 않습니다.");
            }
        }
    }
} 