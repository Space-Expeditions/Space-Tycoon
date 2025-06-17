using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Header("타일맵 설정")]
    public Tilemap tilemap;
    public TileBase[] terrainTiles;  // 사용할 타일 리스트

    [Header("맵 사이즈")]
    public int width = 43;
    public int height = 21;

    [Header("시드 고정")]
    public int seed = 0;
    public bool useRandomSeed = true;

    void Start()
    {
        //  GenerateMap();
    }

    [ContextMenu("Generate Random Tilemap")]
    public void GenerateMap()
    {
        tilemap.ClearAllTiles();

        if (!tilemap || terrainTiles.Length == 0)
        {
            Debug.LogWarning("타일맵 또는 타일이 설정되지 않았습니다.");
            return;
        }

        if (useRandomSeed)
            seed = Random.Range(0, 99999);

        Random.InitState(seed);

        int recentTileBuffer = 6; // 최근 타일 중복 방지 개수
        int maxAttempts = 10;

        Vector3Int startPos = new Vector3Int(-22, -9, 0);

        TileBase[,] placedTiles = new TileBase[width, height];
        Queue<TileBase> recentTiles = new Queue<TileBase>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase chosenTile = null;
                int attempts = 0;

                do
                {
                    chosenTile = terrainTiles[Random.Range(0, terrainTiles.Length)];
                    attempts++;

                    // 중복 검사: 왼쪽, 아래, 최근 N개
                    bool sameAsLeft = (x > 0 && placedTiles[x - 1, y] == chosenTile);
                    bool sameAsBelow = (y > 0 && placedTiles[x, y - 1] == chosenTile);
                    bool inRecentQueue = recentTiles.Contains(chosenTile);

                    if (!sameAsLeft && !sameAsBelow && !inRecentQueue)
                        break;

                } while (attempts < maxAttempts);

                // 배치 및 기록
                placedTiles[x, y] = chosenTile;
                tilemap.SetTile(new Vector3Int(startPos.x + x, startPos.y + y, 0), chosenTile);

                // 최근 타일 큐 업데이트
                recentTiles.Enqueue(chosenTile);
                if (recentTiles.Count > recentTileBuffer)
                    recentTiles.Dequeue();
            }
        }

        Debug.Log("랜덤 타일맵 생성 완료 (Seed: " + seed + ")");
    }
}