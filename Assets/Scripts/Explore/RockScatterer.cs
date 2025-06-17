using UnityEngine;
using UnityEngine.Tilemaps;

public class RockGroupScatterer : MonoBehaviour
{
    [Header("Ÿ�ϸ�")]
    public Tilemap tilemap;

    [Header("�� �׷�")]
    public TileBase[] rockTilesA;   // ȸ�� �� ��
    public TileBase[] rockTilesB;   // ���� �� ��
    public TileBase[] rareRocks;    // ��� ��

    [Header("�� ����")]
    public Vector2Int mapSize = new Vector2Int(43, 21);
    public Vector3Int startPos = new Vector3Int(-2, 1, 0);

    [Header("�е�/Ȯ�� ����")]
    [Range(0f, 1f)] public float minSpawnProbability = 0.05f;
    [Range(0f, 1f)] public float maxSpawnProbability = 0.7f;
    public float centerFalloff = 1.5f;
    [Range(0f, 1f)] public float rareRockChance = 0.05f; // ��� �� ���� Ȯ��

    [ContextMenu("Scatter Rocks")]
    public void ScatterRocks()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap�� �������� �ʾҽ��ϴ�.");
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

                    // �⺻ �׷� ���� (A �Ǵ� B)
                    if (Random.value < 0.5f && rockTilesA.Length > 0)
                        selected = rockTilesA[Random.Range(0, rockTilesA.Length)];
                    else if (rockTilesB.Length > 0)
                        selected = rockTilesB[Random.Range(0, rockTilesB.Length)];

                    // ��� ���� ���� Ȯ��
                    if (rareRocks.Length > 0 && Random.value < rareRockChance)
                        selected = rareRocks[Random.Range(0, rareRocks.Length)];

                    tilemap.SetTile(tilePos, selected);
                }
            }
        }

        Debug.Log("�� Ÿ�� ��ġ �Ϸ�");
    }
}