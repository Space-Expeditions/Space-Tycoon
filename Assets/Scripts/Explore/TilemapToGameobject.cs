using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToGameobject : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] mineralPrefabs;
    public GameObject[] plantPrefabs;

    [Header("ETC")]
    public Tilemap tilemap;
    public ZoneObjectScatter scatter;
    public RockGroupScatterer rockScatter;

    [ContextMenu("Replace Objects")]
    public void ReplaceObjects()
    {
        if (scatter != null)
        {
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(pos);
                if (tile == null) continue;

                int mineralIndex = -1;
                int plantIndex = -1;

                for (int i = 0; i < scatter.mineralTiles.Length; i++)
                {
                    if (scatter.mineralTiles[i] != null && scatter.mineralTiles[i].name == tile.name)
                    {
                        mineralIndex = i;
                        break;
                    }
                }

                if (mineralIndex == -1)
                {
                    for (int i = 0; i < scatter.plantTiles.Length; i++)
                    {
                        if (scatter.plantTiles[i] != null && scatter.plantTiles[i].name == tile.name)
                        {
                            plantIndex = i;
                            break;
                        }
                    }
                }

                if (mineralIndex >= 0)
                {
                    Debug.Log($"Mineral Tile Index {mineralIndex} at {pos}");

                    Vector3 worldPos = tilemap.CellToWorld(pos);
                    worldPos += new Vector3(0.5f, 0.5f, 0);
                    Instantiate(mineralPrefabs[mineralIndex], worldPos, Quaternion.identity, this.transform);
                }
                else if (plantIndex >= 0)
                {
                    Debug.Log($"Plant Tile Index {plantIndex} at {pos}");

                    Vector3 worldPos = tilemap.CellToWorld(pos);
                    worldPos += new Vector3(0.5f, 0.5f, 0);
                    Instantiate(plantPrefabs[plantIndex], worldPos, Quaternion.identity, this.transform);
                }
                else
                {
                    Debug.Log($"Unknown Tile at {pos}");
                }
            }
        }
    }

    [ContextMenu("Replace Objects For Rockscatter")]
    public void ReplaceObjectsForRockscatter()
    {
        if (rockScatter != null)
        {
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(pos);
                if (tile == null) continue;

                int mineralIndex = -1;
                int plantIndex = -1;

                for (int i = 0; i < rockScatter.rockTilesA.Length; i++)
                {
                    if (rockScatter.rockTilesA[i] != null && rockScatter.rockTilesA[i].name == tile.name)
                    {
                        mineralIndex = i;
                        break;
                    }
                }

                if (mineralIndex == -1)
                {
                    for (int i = 0; i < rockScatter.rockTilesB.Length; i++)
                    {
                        if (rockScatter.rockTilesB[i] != null && rockScatter.rockTilesB[i].name == tile.name)
                        {
                            plantIndex = i;
                            break;
                        }
                    }
                }

                if (mineralIndex >= 0)
                {
                    Debug.Log($"Mineral Tile Index {mineralIndex} at {pos}");

                    Vector3 worldPos = tilemap.CellToWorld(pos);
                    worldPos += new Vector3(0.5f, 0.5f, 0);
                    Instantiate(mineralPrefabs[mineralIndex], worldPos, Quaternion.identity, this.transform);
                }
                else if (plantIndex >= 0)
                {
                    Debug.Log($"Plant Tile Index {plantIndex} at {pos}");

                    Vector3 worldPos = tilemap.CellToWorld(pos);
                    worldPos += new Vector3(0.5f, 0.5f, 0);
                    Instantiate(plantPrefabs[plantIndex], worldPos, Quaternion.identity, this.transform);
                }
                else
                {
                    Debug.Log($"Unknown Tile at {pos}");
                }
            }
        }
    }
}