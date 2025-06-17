using UnityEngine;

public class DungeonSpawn : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject;

        player.transform.position = transform.position;
    }
}