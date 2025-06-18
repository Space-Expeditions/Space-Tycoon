using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombinationResult
{
    [Header("재료 아이템 2개 (순서 무관)")]
    public Item ingredient1;
    public Item ingredient2;

    [Header("합성 결과 아이템")]
    public Item resultItem;

    // ⛳ 조합 성립 여부 확인용 헬퍼 함수 (SeedCombinerUI에서 사용 가능)
    public bool Matches(string seedIdA, string seedIdB)
    {
        if (ingredient1 == null || ingredient2 == null) return false;

        var ids = new List<string> { ingredient1.Name, ingredient2.Name };
        var input = new List<string> { seedIdA, seedIdB };

        ids.Sort();
        input.Sort();

        return ids[0] == input[0] && ids[1] == input[1];
    }
}
