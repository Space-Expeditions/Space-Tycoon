using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Header("Ÿ�ϸ� ����")]
    public Tilemap tilemap;
    public TileBase[] terrainTiles;  // ����� Ÿ�� ����Ʈ

    [Header("�� ������")]
    public int width = 43;
    public int height = 21;

    [Header("�õ� ����")]
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
            Debug.LogWarning("Ÿ�ϸ� �Ǵ� Ÿ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        if (useRandomSeed)
            seed = Random.Range(0, 99999);

        Random.InitState(seed);

        int recentTileBuffer = 6; // �ֱ� Ÿ�� �ߺ� ���� ����
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

                    // �ߺ� �˻�: ����, �Ʒ�, �ֱ� N��
                    bool sameAsLeft = (x > 0 && placedTiles[x - 1, y] == chosenTile);
                    bool sameAsBelow = (y > 0 && placedTiles[x, y - 1] == chosenTile);
                    bool inRecentQueue = recentTiles.Contains(chosenTile);

                    if (!sameAsLeft && !sameAsBelow && !inRecentQueue)
                        break;

                } while (attempts < maxAttempts);

                // ��ġ �� ���
                placedTiles[x, y] = chosenTile;
                tilemap.SetTile(new Vector3Int(startPos.x + x, startPos.y + y, 0), chosenTile);

                // �ֱ� Ÿ�� ť ������Ʈ
                recentTiles.Enqueue(chosenTile);
                if (recentTiles.Count > recentTileBuffer)
                    recentTiles.Dequeue();
            }
        }

        Debug.Log("���� Ÿ�ϸ� ���� �Ϸ� (Seed: " + seed + ")");
    }
}