using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScatterer : MonoBehaviour
{
    [Header("Ÿ�ϸ� �� Ÿ��")]
    public Tilemap tilemap;
    public TileBase[] tilesToPlace;  // ���� ���� Ÿ���� �޵��� ����

    [Header("��ġ ����")]
    public int numberOfTiles = 10;
    public int minDistance = 3;  // �ٸ� Ÿ�ϰ��� �ּ� �Ÿ�
    public Vector2Int mapSize = new Vector2Int(43, 21);
    public Vector3Int startPos = new Vector3Int(-2, 1, 0);
    public int maxAttempts = 100;

    [ContextMenu("Place Scattered Tiles")]
    public void PlaceTiles()
    {
        tilemap.ClearAllTiles();

        if (tilesToPlace == null || tilesToPlace.Length == 0)
        {
            Debug.LogWarning("Ÿ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        List<Vector3Int> placedPositions = new List<Vector3Int>();

        int attempts = 0;
        int placed = 0;

        while (placed < numberOfTiles && attempts < maxAttempts * numberOfTiles)
        {
            int x = Random.Range(0, mapSize.x);
            int y = Random.Range(0, mapSize.y);
            Vector3Int candidate = startPos + new Vector3Int(x, y, 0);

            // �Ÿ� ���� �˻�
            bool tooClose = false;
            foreach (var pos in placedPositions)
            {
                if (Vector3Int.Distance(candidate, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose && tilemap.GetTile(candidate) == null)
            {
                // ���� ���� Ÿ�� �� �����ϰ� ����
                TileBase chosenTile = tilesToPlace[Random.Range(0, tilesToPlace.Length)];
                tilemap.SetTile(candidate, chosenTile);
                placedPositions.Add(candidate);
                placed++;
            }

            attempts++;
        }

        Debug.Log($"�� {placed}���� Ÿ���� ���������� ��ġ�Ǿ����ϴ�.");
    }
}