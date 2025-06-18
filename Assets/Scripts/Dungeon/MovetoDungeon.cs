using UnityEngine;
using UnityEngine.SceneManagement;

public class MovetoDungeon : MonoBehaviour
{
    public string targetSceneName; // �̵��� �� �̸��� ����Ƽ���� ���� ����
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogWarning("targetSceneName�� �����Ǿ� ���� �ʽ��ϴ�!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
