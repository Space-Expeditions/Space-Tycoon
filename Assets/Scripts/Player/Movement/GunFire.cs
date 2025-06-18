using UnityEngine;

public class GunFire : MonoBehaviour
{
    public Transform firePoint;                // �ѱ� ��ġ
    public GameObject laserEffectPrefab;       // ������ ������
    public float fireRate = 0.3f;              // ���� �ӵ�
    public float laserSpeed = 20f;             // ������ �ӵ�

    private float lastFireTime;

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
