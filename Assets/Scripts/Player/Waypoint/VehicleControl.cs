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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spRenderer = GetComponent<SpriteRenderer>();
        
        player = GameObject.FindAnyObjectByType<PlayerMovement>();
        followCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<FollowCamera>();
        spawnManager = GameObject.FindFirstObjectByType<PlayerSpawnManager>();
        gunController = player?.GetComponentInChildren<GunController>();
        // gunController = player?.transform.GetChild(0).GetComponent<GunController>();
        // gunController = player?.GetComponentsInChildren<GunController>(true).FirstOrDefault();
        
        if (player == null || gunController == null || followCamera == null || spawnManager == null)
        {
            Debug.LogWarning("필수 오브젝트가 씬에 존재하지 않거나 구성 요소가 누락되었습니다.");
            return;
        }

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
                {
                    Riding();
                }
            }
            else
            {
                bool near = false;

                for (int i = 0; i < spawnManager.waypointPos.Count; i++)
                {
                    if (spawnManager.waypointPos[i].GetComponent<Teleporter>().NearTeleporter() && spawnManager.waypointPos[i].GetComponent<Animator>().enabled)
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
        }

        player.canMove = !isRiding;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") && isRiding)
        {
            transform.position = collision.collider.GetComponent<TileWalk>().StepTile(transform);
        }
    }
}