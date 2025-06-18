using UnityEngine;
using UnityEngine.Tilemaps;

public class TileWalk : MonoBehaviour
{
    new FollowCamera camera;
    
    public Transform point;
    public Tilemap tilemap;
    
    public bool isHorizontal = false;

    void Start()
    {
        camera = GameObject.FindAnyObjectByType<FollowCamera>();
    }

    public Vector2 StepTile(Transform playerTransform)
    {
        camera.SetNewTilemap(tilemap);

        Vector2 pos;

        if (isHorizontal)
        {
            pos = new Vector2(point.position.x, playerTransform.position.y);
        }
        else
        {
            pos = new Vector2(playerTransform.position.x, point.position.y);
        }

        return pos;
    }
}