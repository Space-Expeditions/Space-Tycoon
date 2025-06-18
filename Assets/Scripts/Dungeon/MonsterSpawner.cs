using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // Monster1~8 ������ �巡�׷� �ֱ�
    public float spawnInterval = 2f;    // ���� ���� ����

    void Start()
    {
        InvokeRepeating("SpawnMonster", 1f, spawnInterval);
    }

    void SpawnMonster()
    {
        if (monsterPrefabs.Length == 0)
        {
            Debug.LogWarning("몬스터가 스폰되었습니다!");
            return;
        }

        GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        Vector3 spawnPosition = transform.position;

        GameObject monster = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log(monster.name + " 스폰!");
    }
}
