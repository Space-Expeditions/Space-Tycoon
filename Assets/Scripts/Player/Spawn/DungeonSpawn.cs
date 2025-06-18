using UnityEngine;

public class DungeonSpawn : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>()?.gameObject;
        if (player == null)
        {
            Debug.LogWarning("필수 오브젝트가 씬에 존재하지 않습니다.");
            return;
        }

        player.transform.position = transform.position;
    }
}