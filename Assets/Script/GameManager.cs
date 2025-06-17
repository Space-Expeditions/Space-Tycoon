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
            DontDestroyOnLoad(gameObject);  // �ı� �� �ǰ� ����
            SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �̺�Ʈ ���
        }
        else
        {
            Destroy(gameObject);  // �ߺ� ����
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;  // �̺�Ʈ ����
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dragAndDropController = GameObject.FindFirstObjectByType<ItemDragAndDropController>();

        // �Ǵ�, �ӵ� �켱�̸�
        // dragAndDropController = GameObject.FindAnyObjectByType<ItemDragAndDropController>();
    }
}

