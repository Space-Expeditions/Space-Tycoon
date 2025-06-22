using UnityEngine;

public class GunFire : MonoBehaviour
{
    public Transform firePoint;                // 총구 위치
    public GameObject laserEffectPrefab;       // 레이저 프리팹
    public float fireRate = 0.3f;              // 발사 속도
    public float laserSpeed = 20f;             // 레이저 속도

    private float lastFireTime;

    public AudioClip fireSound;                // 총소리 클립
    private AudioSource audioSource;           // 소리 재생기

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Fire()
    {
        if (Time.time - lastFireTime < fireRate)
            return;

        lastFireTime = Time.time;

        // 총알 생성
        GameObject laser = Instantiate(laserEffectPrefab, firePoint.position, firePoint.rotation);
        laser.tag = "Laser";

        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * laserSpeed; // ← 여기 velocity로 수정함
        }

        Destroy(laser, 2f);

        // 🔊 총소리 재생
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }
}
