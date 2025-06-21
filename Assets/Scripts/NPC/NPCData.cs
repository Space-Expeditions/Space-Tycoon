using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed,
    Rejected,
    Failed
}

[Serializable]
public class QuestRequirement
{
    public string itemName;
    public int count;
    [NonSerialized] public int currentCount;
}

[CreateAssetMenu(menuName = "Data/NPCData")]
public class NPCData : ScriptableObject
{
    public string questName;
    public string npcName;
    public string description;
    public List<QuestRequirement> requiredItems;
    public string rewardDescription;
    public QuestState state;
    public string rewardItemName;
    public int rewardItemCount;
    
    /// <summary>
    /// 예시 대화 시나리오
    /// </summary>
    public static Dictionary<string, List<string>> npcScenarios = new Dictionary<string, List<string>>()
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
        }},
        { "우주펭귄", new List<string> {
            "펭귄입니다! 실험실에서 연구 중이에요.",
            "아메시스트가 필요해요. 혹시 가지고 있나요?",
            "다이아몬드도 필요해요. 찾아주시면 감사하겠어요!",
            "실험은 항상 흥미로워요. 새로운 발견이 기대돼요!",
            "우주에서 펭귄을 만나다니 신기하죠?"
        }}
    };
    
    /// <summary>
    /// 예시 NPC 데이터 (퀘스트 & 시나리오)
    /// </summary>
    public static List<NPCData> GetSampleNPCs()
    {
        var list = new List<NPCData>();
        
        var n1 = ScriptableObject.CreateInstance<NPCData>();
        n1.questName = "우주 농사의 첫걸음";
        n1.npcName = "외계인";
        n1.description = "외계인에게 당근, 감자, 토마토를 하나씩 가져다 주세요.";
        n1.requiredItems = new List<QuestRequirement> {
            new QuestRequirement { itemName = "Carrot", count = 1 },
            new QuestRequirement { itemName = "Potato", count = 1 },
            new QuestRequirement { itemName = "Tomato", count = 1 }
        };
        n1.rewardDescription = "뼈다귀 1개";
        n1.state = QuestState.NotStarted;
        n1.rewardItemName = "Bone";
        n1.rewardItemCount = 1;
        list.Add(n1);
        
        var n2 = ScriptableObject.CreateInstance<NPCData>();
        n2.questName = "광부의 부탁";
        n2.npcName = "우주 광부";
        n2.description = "광부에게 구리 광석 5개를 가져다 주세요.";
        n2.requiredItems = new List<QuestRequirement> { new QuestRequirement { itemName = "CopperOre", count = 5 } };
        n2.rewardDescription = "뼈다귀 2개";
        n2.state = QuestState.NotStarted;
        n2.rewardItemName = "Bone";
        n2.rewardItemCount = 2;
        list.Add(n2);
        
        var n3 = ScriptableObject.CreateInstance<NPCData>();
        n3.questName = "탐험가의 스프레이 찾기";
        n3.npcName = "우주 탐험가";
        n3.description = "탐험가에게 스프레이 3개를 가져다 주세요.";
        n3.requiredItems = new List<QuestRequirement> { new QuestRequirement { itemName = "Spray", count = 3 } };
        n3.rewardDescription = "뼈다귀 3개";
        n3.rewardItemName = "Bone";
        n3.rewardItemCount = 3;
        n3.state = QuestState.NotStarted;
        list.Add(n3);
        
        var n4 = ScriptableObject.CreateInstance<NPCData>();
        n4.questName = "펭귄의 실험";
        n4.npcName = "우주펭귄";
        n4.description = "펭귄에게 아메시스트 1개를 가져다 주세요.";
        n4.requiredItems = new List<QuestRequirement> { new QuestRequirement { itemName = "AmethystOre", count = 1 } };
        n4.rewardDescription = "뼈다귀 4개";
        n4.rewardItemName = "Bone";
        n4.rewardItemCount = 4;
        n4.state = QuestState.NotStarted;
        list.Add(n4);
        
        // var n5 = ScriptableObject.CreateInstance<NPCData>();
        // n5.questName = "요리사에게 식재료 가져다주기";
        // n5.npcName = "우주 요리사";
        // n5.description = "요리사에게 당근 2개와 감자 2개를 가져다 주세요.";
        // n5.requiredItems = new List<QuestRequirement> {
        //     new QuestRequirement { itemName = "Carrot", count = 2 },
        //     new QuestRequirement { itemName = "Potato", count = 2 }
        // };
        // n5.rewardDescription = "뼈다귀 5개";
        // n5.rewardItemName = "Bone";
        // n5.rewardItemCount = 5;
        // n5.state = QuestState.NotStarted;
        // list.Add(n5);
        
        return list;
    }
} 