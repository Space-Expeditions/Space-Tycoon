using UnityEngine;

public class DungeonSpawn : MonoBehaviour
{
    FollowCamera followCamera;
    
    GameObject player;
    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>()?.gameObject;
        followCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<FollowCamera>();

        if (player == null || followCamera == null)
        {
            Debug.LogWarning("필수 오브젝트가 씬에 존재하지 않습니다.");
            return;
        }
        
        followCamera.target = player.transform;
        player.transform.position = transform.position;
    }
}