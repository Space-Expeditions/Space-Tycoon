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
    
    /// <summary>
    /// 예시 대화 시나리오
    /// </summary>
    private Dictionary<string, List<string>> npcScenarios = new Dictionary<string, List<string>>()
    {
        { "외계인", new List<string> {
            "어서 와, 지구인! 이 은하계에서 살아남으려면 보석을 잘 모아둬야 해.",
            "보석은 잘 모아두면 분명 쓸모가 있을 거야. 탈출하려면 꼭 필요해!",
            "혹시 보석을 찾으면 나에게 가져와줘. 좋은 정보를 줄게!",
            "이 행성의 밤은 위험해. 조심해서 다녀!",
            "내가 아는 전설에 따르면, 보석을 모두 모으면 특별한 일이 일어난대."
        }},
        { "우주 광부", new List<string> {
            "광산은 위험하니 조심해! 구리 광석을 많이 모아오면 보상을 줄게.",
            "채굴은 쉽지 않지만, 인내심이 있다면 큰 보상을 얻을 수 있어.",
            "도구가 필요하면 언제든 나를 찾아와!",
            "깊은 곳일수록 더 귀한 광물이 숨어있지.",
            "광산에서 길을 잃지 않게 항상 횃불을 챙겨!"
        }},
        { "우주 탐험가", new List<string> {
            "이 행성에는 숨겨진 비밀이 많아. 지도를 완성해보는 건 어때?",
            "탐험은 위험하지만, 그만큼 보람도 크지!",
            "새로운 지역을 발견하면 꼭 나에게 알려줘!",
            "지도에 없는 장소를 찾으면 큰 보상을 받을 수 있어.",
            "탐험 중에 이상한 소리를 들으면 주의해!"
        }}
    };

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

    public void ShowMessage(string npcName)
    {
        if (!npcScenarios.ContainsKey(npcName))
        {
            scenarioMessages = new List<string> { "..." };
        }
        else
        {
            // 시나리오 중 랜덤으로 하나만 선택
            var all = npcScenarios[npcName];
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
            // 대화 하나만 보여주고 클릭/Backspace 시 바로 닫기
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