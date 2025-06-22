using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleControl : MonoBehaviour
{
    public GameObject point;

    Rigidbody2D rb;

    public Sprite noPlayerVehicle;
    public Sprite onPlayerVehicle;

    SpriteRenderer spRenderer;

    PlayerMovement player;
    FollowCamera followCamera;
    PlayerSpawnManager spawnManager;
    GunController gunController;

    public float vehicleSpeed = 7f;
    public bool canMove = true;
    public bool isRiding;

    // 🔊 차량 부릉부릉 소리
    public AudioClip vehicleEngineSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindAnyObjectByType<PlayerMovement>();
        followCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<FollowCamera>();
        spawnManager = GameObject.FindFirstObjectByType<PlayerSpawnManager>();
        gunController = player?.GetComponentInChildren<GunController>();

        // 🔊 오디오 소스 설정
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = vehicleEngineSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (isRiding)
        {
            spRenderer.sprite = onPlayerVehicle;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            followCamera.target = transform;

            player.GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            if (gunController.isEquip)
                gunController.ToggleGun();
            else
                gunController.SetGunRenderer();
            player.GetComponent<PlayerMovement>().enabled = false;

            if (vehicleEngineSound != null)
                audioSource.Play(); // 시작 시 탑승 상태면 소리 재생
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!isRiding)
            {
                float distance = Vector2.Distance(player.transform.position, transform.position);
                if (distance <= 2f)
                    Riding();
            }
            else
            {
                bool near = false;

                for (int i = 0; i < spawnManager.waypointPos.Count; i++)
                {
                    if (spawnManager.waypointPos[i].GetComponent<Teleporter>().NearTeleporter() &&
                        spawnManager.waypointPos[i].GetComponent<Animator>().enabled)
                    {
                        near = true;
                        break;
                    }
                }

                if (!near)
                    Riding();
                else
                {
                    if (!spawnManager.grid.activeSelf)
                    {
                        spawnManager.grid.SetActive(true);
                        spawnManager.grid.transform.parent.gameObject.GetComponent<ButtonGrid>().SetWaypointButtons();
                        canMove = false;
                    }
                    else
                    {
                        spawnManager.grid.SetActive(false);
                        canMove = true;
                    }
                }
            }
        }

        if (isRiding && canMove)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            Vector3 move = new Vector3(moveX, moveY, 0f).normalized;
            transform.position += move * vehicleSpeed * Time.deltaTime;

            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.down, move);
                transform.rotation = targetRotation;
            }

            player.transform.position = transform.position;
        }
    }

    public void Riding()
    {
        if (!isRiding)
        {
            isRiding = true;
            spRenderer.sprite = onPlayerVehicle;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            followCamera.target = transform;

            player.GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            if (gunController.isEquip)
                gunController.ToggleGun();
            else
                gunController.SetGunRenderer();
            player.GetComponent<PlayerMovement>().enabled = false;

            if (vehicleEngineSound != null)
                audioSource.Play(); // 🚗 소리 재생
        }
        else
        {
            isRiding = false;
            spRenderer.sprite = noPlayerVehicle;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            followCamera.target = player.transform;
            player.transform.position = point.transform.position;

            player.GetComponent<BoxCollider2D>().enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = true;
            gunController.SetGunRenderer();
            player.GetComponent<PlayerMovement>().enabled = true;

            audioSource.Stop(); // 🛑 소리 정지
        }

        player.canMove = !isRiding;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            transform.position = collision.collider.GetComponent<TileWalk>().StepTile(transform);
        }
    }
}
