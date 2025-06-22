using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform player;
    public float offsetDistance = 0.5f;
    private SpriteRenderer gunRenderer;
    public Transform firePoint;

    public bool isEquip;

    void Start()
    {
        gunRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - player.position).normalized;

        // 총 위치 갱신
        transform.position = player.position + direction * offsetDistance;

        // 총 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 🔁 총 좌우 반전 처리
        if (direction.x < 0)
        {
            gunRenderer.flipY = true; // 왼쪽 보면 뒤집음

            firePoint.localPosition = new Vector3(firePoint.localPosition.x, -1.85f, 0);
        }
        else
        {
            gunRenderer.flipY = false;

            firePoint.localPosition = new Vector3(firePoint.localPosition.x, -1.75f, 0);
        }

        // 🔽 총이 플레이어 앞/뒤에 보이도록 처리
        if (direction.y > 0)
        {
            gunRenderer.sortingOrder = 4; // 뒤로 감
        }
        else
        {
            gunRenderer.sortingOrder = 6; // 앞으로 나옴
        }
    }

    public void ToggleGun()
    {
        isEquip = !isEquip;

        SetGunRenderer();
    }

    public void SetGunRenderer()
    {
        if (isEquip)
        {
            gunRenderer.enabled = true;
        }
        else
        {
            gunRenderer.enabled = false;
        }
    }
}