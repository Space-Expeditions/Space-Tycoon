using UnityEngine;
using UnityEngine.Tilemaps;

public class RockGroupScatterer : MonoBehaviour
{
    [Header("타일맵")]
    public Tilemap tilemap;

    [Header("돌 그룹")]
    public TileBase[] rockTilesA;   // 회색 돌 등
    public TileBase[] rockTilesB;   // 갈색 돌 등
    public TileBase[] rareRocks;    // 희귀 돌

    [Header("맵 설정")]
    public Vector2Int mapSize = new Vector2Int(43, 21);
    public Vector3Int startPos = new Vector3Int(-2, 1, 0);

    [Header("밀도/확률 조정")]
    [Range(0f, 1f)] public float minSpawnProbability = 0.05f;
    [Range(0f, 1f)] public float maxSpawnProbability = 0.7f;
    public float centerFalloff = 1.5f;
    [Range(0f, 1f)] public float rareRockChance = 0.05f; // 희귀 돌 등장 확률

    [ContextMenu("Scatter Rocks")]
    public void ScatterRocks()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap이 설정되지 않았습니다.");
            return;
        }

        tilemap.ClearAllTiles();

        Vector2 center = new Vector2(mapSize.x / 2f, mapSize.y / 2f);
        float maxDistance = Vector2.Distance(Vector2.zero, center);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector2 pos = new Vector2(x, y);
                float distToCenter = Vector2.Distance(pos, center);

                float t = Mathf.Pow(distToCenter / maxDistance, centerFalloff);
                float spawnChance = Mathf.Lerp(minSpawnProbability, maxSpawnProbability, t);

                if (Random.value < spawnChance)
                {
                    Vector3Int tilePos = startPos + new Vector3Int(x, y, 0);
                    TileBase selected = null;

                    // 기본 그룹 선택 (A 또는 B)
                    if (Random.value < 0.5f && rockTilesA.Length > 0)
                        selected = rockTilesA[Random.Range(0, rockTilesA.Length)];
                    else if (rockTilesB.Length > 0)
                        selected = rockTilesB[Random.Range(0, rockTilesB.Length)];

                    // 희귀 돌로 덮을 확률
                    if (rareRocks.Length > 0 && Random.value < rareRockChance)
                        selected = rareRocks[Random.Range(0, rareRocks.Length)];

                    tilemap.SetTile(tilePos, selected);
                }
            }
        }

        Debug.Log("돌 타일 배치 완료");
    }
}
