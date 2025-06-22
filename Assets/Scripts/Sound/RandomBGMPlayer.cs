using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    public AudioClip[] bgmClips; // ���� BGM Ŭ�� �迭
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (bgmClips.Length == 0)
        {
            Debug.LogWarning("BGM Ŭ���� �����ϴ�!");
            return;
        }

        PlayRandomBGM();
    }

    void PlayRandomBGM()
    {
        int randomIndex = Random.Range(0, bgmClips.Length);
        audioSource.clip = bgmClips[randomIndex];
        audioSource.Play();
    }

    // ���� BGM�� ������ �ڵ����� ������ ����ϰ� �ʹٸ� �Ʒ� �Լ� �߰� ����
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomBGM();
        }
    }
}
