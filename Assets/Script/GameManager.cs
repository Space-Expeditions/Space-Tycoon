using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public InventoryPanel inventoryPanel;
    public ItemPanel toolbarPanel;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemContainer toolbarContainer;
    public ItemDragAndDropController dragAndDropController;
    public GameObject mapGrid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 파괴 안 되게 설정
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 등록
        }
        else
        {
            Destroy(gameObject);  // 중복 방지
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;  // 이벤트 해제
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dragAndDropController = GameObject.FindFirstObjectByType<ItemDragAndDropController>();

        // 또는, 속도 우선이면
        // dragAndDropController = GameObject.FindAnyObjectByType<ItemDragAndDropController>();
    }
}

