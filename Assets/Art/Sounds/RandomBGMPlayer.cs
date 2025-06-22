using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    public AudioClip[] bgmClips; // 여러 BGM 클립 배열
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (bgmClips.Length == 0)
        {
            Debug.LogWarning("BGM 클립이 없습니다!");
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

    // 만약 BGM이 끝나면 자동으로 다음곡 재생하고 싶다면 아래 함수 추가 가능
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomBGM();
        }
    }
}