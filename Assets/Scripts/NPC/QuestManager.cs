using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance { get; private set; }
    public List<NPCData> quests = new List<NPCData>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);
        InitSampleQuests();
    }

    private void InitSampleQuests()
    {
        quests = NPCData.GetSampleNPCs();
    }

    public List<NPCData> GetQuestsByNPC(string npcName)
    {
        return quests.FindAll(q => q.npcName == npcName);
    }

    public void AcceptQuest(NPCData quest)
    {
        if (quest.state == QuestState.NotStarted || quest.state == QuestState.Rejected)
            quest.state = QuestState.InProgress;
    }

    public void RejectQuest(NPCData quest)
    {
        if (quest.state == QuestState.NotStarted || quest.state == QuestState.InProgress)
            quest.state = QuestState.Rejected;
    }

    public void CompleteQuest(NPCData quest)
    {
        if (quest.state == QuestState.InProgress)
            quest.state = QuestState.Completed;
    }

    public void FailQuest(NPCData quest)
    {
        if (quest.state == QuestState.InProgress)
            quest.state = QuestState.Failed;
    }

    public List<NPCData> GetAllQuests()
    {
        return quests;
    }
} 