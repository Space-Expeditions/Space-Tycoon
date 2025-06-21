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
        
        var n5 = ScriptableObject.CreateInstance<NPCData>();
        n5.questName = "탐험가의 스프레이 찾기";
        n5.npcName = "우주 탐험가";
        n5.description = "탐험가에게 스프레이 3개를 가져다 주세요.";
        n5.requiredItems = new List<QuestRequirement> { new QuestRequirement { itemName = "Spray", count = 3 } };
        n5.rewardDescription = "뼈다귀 5개";
        n5.rewardItemName = "Bone";
        n5.rewardItemCount = 5;
        n5.state = QuestState.NotStarted;
        list.Add(n5);

        // var n3 = ScriptableObject.CreateInstance<NPCData>();
        // n3.questName = "요리사에게 식재료 가져다주기";
        // n3.npcName = "우주 요리사";
        // n3.description = "요리사에게 당근 2개와 감자 2개를 가져다 주세요.";
        // n3.requiredItems = new List<QuestRequirement> {
        //     new QuestRequirement { itemName = "Carrot", count = 2 },
        //     new QuestRequirement { itemName = "Potato", count = 2 }
        // };
        // n3.rewardDescription = "뼈다귀 3개";
        // n3.rewardItemName = "Bone";
        // n3.rewardItemCount = 3;
        // n3.state = QuestState.NotStarted;
        // list.Add(n3);
        
        // var n4 = ScriptableObject.CreateInstance<NPCData>();
        // n4.questName = "과학자의 실험";
        // n4.npcName = "우주 과학자";
        // n4.description = "과학자에게 아메시스트 1개를 가져다 주세요.";
        // n4.requiredItems = new List<QuestRequirement> { new QuestRequirement { itemName = "Amethyst", count = 1 } };
        // n4.rewardDescription = "뼈다귀 4개";
        // n4.rewardItemName = "Bone";
        // n4.rewardItemCount = 4;
        // n4.state = QuestState.NotStarted;
        // list.Add(n4);

        return list;
    }
} 