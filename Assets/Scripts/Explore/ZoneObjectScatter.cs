using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneObjectScatter : MonoBehaviour
{
    [Header("타일맵")]
    public Tilemap tilemap;

    [Header("타일 설정")]
    public TileBase[] mineralTiles;
    public TileBase[] plantTiles;

    [Header("맵 설정")]
    public Vector2Int mapSize = new Vector2Int(43, 21);
    public Vector3Int startPos = new Vector3Int(-2, 1, 0);

    [Header("배치 밀도 조절")]
    [Range(0f, 1f)] public float tileDensityPerZone = 0.4f;
    [Range(0f, 0.1f)] public float ambientDensity = 0.02f;
    [Range(0f, 1f)] public float blendChance = 0.3f;

    [ContextMenu("Scatter Tiles in Zones")]
    public void ScatterTiles()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap이 비어 있습니다.");
            return;
        }

        tilemap.ClearAllTiles();

        Vector2Int zoneSize = new Vector2Int(mapSize.x / 2, mapSize.y / 2);
        List<RectInt> zones = new List<RectInt>()
        {
            new RectInt(0, 0, zoneSize.x, zoneSize.y),
            new RectInt(zoneSize.x, 0, zoneSize.x, zoneSize.y),
            new RectInt(0, zoneSize.y, zoneSize.x, zoneSize.y),
            new RectInt(zoneSize.x, zoneSize.y, zoneSize.x, zoneSize.y),
        };

        List<int> selectedZoneIndices = new List<int>();
        while (selectedZoneIndices.Count < 3)
        {
            int idx = Random.Range(0, zones.Count);
            if (!selectedZoneIndices.Contains(idx))
                selectedZoneIndices.Add(idx);
        }

        List<string> zoneTypes = new List<string>();
        zoneTypes.Add("Mineral");
        zoneTypes.Add("Plant");
        zoneTypes.Add(Random.value < 0.5f ? "Mineral" : "Plant");
        zoneTypes.Shuffle();

        for (int i = 0; i < selectedZoneIndices.Count; i++)
        {
            RectInt zone = zones[selectedZoneIndices[i]];
            string type = zoneTypes[i];
            int totalTiles = zone.width * zone.height;
            int tilesToPlace = Mathf.RoundToInt(totalTiles * tileDensityPerZone);

            HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();

            for (int t = 0; t < tilesToPlace; t++)
            {
                Vector2Int pos;
                int attempt = 0;
                do
                {
                    pos = new Vector2Int(
                        Random.Range(zone.xMin, zone.xMax),
                        Random.Range(zone.yMin, zone.yMax));
                    attempt++;
                } while (usedPositions.Contains(pos) && attempt < 50);

                usedPositions.Add(pos);
                Vector3Int tilePos = startPos + new Vector3Int(pos.x, pos.y, 0);

                TileBase tile = type == "Mineral"
                    ? GetRandomTile(mineralTiles)
                    : GetRandomTile(plantTiles);

                if (tile != null)
                    tilemap.SetTile(tilePos, tile);
            }

            Debug.Log($"Zone {selectedZoneIndices[i]} → {type} 구역");
        }

        // 인접한 구역 간 경계 혼합 처리
        for (int i = 0; i < selectedZoneIndices.Count; i++)
        {
            for (int j = i + 1; j < selectedZoneIndices.Count; j++)
            {
                RectInt zoneA = zones[selectedZoneIndices[i]];
                RectInt zoneB = zones[selectedZoneIndices[j]];
                string typeA = zoneTypes[i];
                string typeB = zoneTypes[j];
                BlendBetweenZones(zoneA, typeA, zoneB, typeB);
            }
        }

        for (int i = 0; i < zones.Count; i++)
        {
            if (selectedZoneIndices.Contains(i)) continue;
            RectInt zone = zones[i];

            for (int x = zone.xMin; x < zone.xMax; x++)
            {
                for (int y = zone.yMin; y < zone.yMax; y++)
                {
                    if (Random.value < ambientDensity)
                    {
                        Vector3Int tilePos = startPos + new Vector3Int(x, y, 0);
                        TileBase tile = Random.value < 0.5f
                            ? GetRandomTile(mineralTiles)
                            : GetRandomTile(plantTiles);

                        if (tile != null)
                            tilemap.SetTile(tilePos, tile);
                    }
                }
            }
        }

        Debug.Log("✅ 타일 배치 완료");
    }

    private TileBase GetRandomTile(TileBase[] tiles)
    {
        if (tiles == null || tiles.Length == 0) return null;
        return tiles[Random.Range(0, tiles.Length)];
    }

    private void BlendBetweenZones(RectInt zoneA, string typeA, RectInt zoneB, string typeB)
    {
        bool horizontallyAdjacent = zoneA.yMin == zoneB.yMin && zoneA.yMax == zoneB.yMax &&
                                    (zoneA.xMax == zoneB.xMin || zoneB.xMax == zoneA.xMin);
        bool verticallyAdjacent = zoneA.xMin == zoneB.xMin && zoneA.xMax == zoneB.xMax &&
                                  (zoneA.yMax == zoneB.yMin || zoneB.yMax == zoneA.yMin);

        if (!horizontallyAdjacent && !verticallyAdjacent) return;

        for (int x = Mathf.Max(zoneA.xMin, zoneB.xMin); x < Mathf.Min(zoneA.xMax, zoneB.xMax); x++)
        {
            for (int y = Mathf.Max(zoneA.yMin, zoneB.yMin); y < Mathf.Min(zoneA.yMax, zoneB.yMax); y++)
            {
                if (Random.value < blendChance)
                {
                    Vector3Int tilePos = startPos + new Vector3Int(x, y, 0);
                    string useType = Random.value < 0.5f ? typeA : typeB;

                    TileBase tile = useType == "Mineral"
                        ? GetRandomTile(mineralTiles)
                        : GetRandomTile(plantTiles);

                    tilemap.SetTile(tilePos, tile);
                }
            }
        }
    }
}

// List 확장 메서드: 무작위 셔플
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int r = Random.Range(i, n);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}