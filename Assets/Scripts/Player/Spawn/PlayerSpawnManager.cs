using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawnManager : MonoBehaviour
{
    new FollowCamera camera;
    
    WaypointManager waypointManager;
    VehicleControl player;
    
    public GameObject grid;
    public List<Tilemap> maps = new List<Tilemap>();
    public List<Transform> waypointPos = new List<Transform>();

    void Start()
    {
        grid = GameObject.FindFirstObjectByType<InventoryManager>()?.mapGrid;
        camera = GameObject.FindAnyObjectByType<FollowCamera>();
        waypointManager = GameObject.FindAnyObjectByType<WaypointManager>();
        player = GameObject.FindAnyObjectByType<VehicleControl>();
        if (grid == null || camera== null || player == null || waypointManager == null)
        {
            Debug.LogWarning("필수 오브젝트가 씬에 존재하지 않습니다.");
            return;
        }

        if (waypointManager.selecePointNum != -1)
        {
            camera.SetNewTilemap(maps[waypointManager.selecePointNum]);
            player.transform.position = waypointPos[waypointManager.selecePointNum].position;
        }

        grid.SetActive(false);
    }

    public void Warp(int index)
    {
        camera.SetNewTilemap(maps[index]);
        player.transform.position = waypointPos[index].position;
        player.canMove = true;
    }
}