using UnityEngine;

public class GunFire : MonoBehaviour
{
    public Transform firePoint;                // 총구 위치
    public GameObject laserEffectPrefab;       // 레이저 프리팹
    public float fireRate = 0.3f;              // 연사 속도
    public float laserSpeed = 20f;             // 레이저 속도

    private float lastFireTime;

    SpriteRenderer gunSR;

    void Start()
    {
        gunSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (gunSR.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        GameObject laser = Instantiate(laserEffectPrefab, firePoint.position, firePoint.rotation);
        laser.tag = "Laser";

        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * laserSpeed;
        }

        Destroy(laser, 2f);
    }
}
