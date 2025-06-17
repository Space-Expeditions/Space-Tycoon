using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScatterer : MonoBehaviour
{
    [Header("타일맵 및 타일")]
    public Tilemap tilemap;
    public TileBase[] tilesToPlace;  // 여러 개의 타일을 받도록 변경

    [Header("배치 설정")]
    public int numberOfTiles = 10;
    public int minDistance = 3;  // 다른 타일과의 최소 거리
    public Vector2Int mapSize = new Vector2Int(43, 21);
    public Vector3Int startPos = new Vector3Int(-2, 1, 0);
    public int maxAttempts = 100;

    [ContextMenu("Place Scattered Tiles")]
    public void PlaceTiles()
    {
        tilemap.ClearAllTiles();

        if (tilesToPlace == null || tilesToPlace.Length == 0)
        {
            Debug.LogWarning("타일이 설정되지 않았습니다.");
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

            // 거리 조건 검사
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
                // 여러 개의 타일 중 랜덤하게 선택
                TileBase chosenTile = tilesToPlace[Random.Range(0, tilesToPlace.Length)];
                tilemap.SetTile(candidate, chosenTile);
                placedPositions.Add(candidate);
                placed++;
            }

            attempts++;
        }

        Debug.Log($"총 {placed}개의 타일이 성공적으로 배치되었습니다.");
    }
}