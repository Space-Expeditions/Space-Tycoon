using UnityEngine;
using System.Linq;

public class NPC : MonoBehaviour
{
    public string npcName;
    public float interactDistance = 2f;
    private GameObject player;

    public AudioClip[] voiceClips; 
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.tag = "NPC";

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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

            if (voiceClips != null && voiceClips.Length > 0)
            {
                int index = Random.Range(0, voiceClips.Length);
                audioSource.Stop();  // 기존 소리 끊기
                audioSource.PlayOneShot(voiceClips[index]);
            }
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
