using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawnManager : MonoBehaviour
{
    public GameObject grid;

    FollowCamera camera;
    WaypointManager waypointManager;
    VehicleControl player;

    public List<Tilemap> maps = new List<Tilemap>();
    public List<Transform> waypointPos = new List<Transform>();

    void Start()
    {
        grid = GameObject.FindFirstObjectByType<GameManager>().mapGrid;
        camera = GameObject.FindAnyObjectByType<FollowCamera>();
        waypointManager = GameObject.FindAnyObjectByType<WaypointManager>();
        player = GameObject.FindAnyObjectByType<VehicleControl>();

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
