using UnityEngine;

public class MineralSoundManager : MonoBehaviour
{
    public static MineralSoundManager instance;

    public AudioClip breakSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayBreakSound(Vector3 position)
    {
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, position);
        }
    }
}
