using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool canMove = true;

    Vector3 move;

    SpriteRenderer spriteRenderer;
    Animator animator;
    WaypointManager waypointManager;

    public Sprite idleFront;
    public Sprite idleBack;
    public Sprite idleSide;

    float lastMoveX = 0;
    float lastMoveY = -1;

    public bool anotherAnim = false;

    static public PlayerMovement instance;

    // 발소리 관련
    public AudioClip footstepSound;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        waypointManager = GameObject.FindFirstObjectByType<WaypointManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 발소리 세팅 (루프, 볼륨 조절)
        audioSource.clip = footstepSound;
        audioSource.loop = true;
        audioSource.volume = 0.5f; // 필요하면 조절

        if (SceneManager.GetActiveScene().buildIndex == 0 && waypointManager.isReturn)
        {
            transform.position = GameObject.FindFirstObjectByType<VehicleControl>().transform.GetChild(0).transform.position;
        }
    }

    void Update()
    {
        move = Vector3.zero;
        bool isMoving = false;

        if (Input.GetKey(KeyCode.W))
        {
            move += Vector3.up;
            lastMoveY = 1;
            lastMoveX = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += Vector3.down;
            lastMoveY = -1;
            lastMoveX = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += Vector3.left;
            lastMoveX = -1;
            lastMoveY = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += Vector3.right;
            lastMoveX = 1;
            lastMoveY = 0;
        }

        move = move.normalized;
        isMoving = move.magnitude > 0;

        if (isMoving)
        {
            animator.enabled = true;

            if (canMove)
            {
                animator.SetFloat("MoveX", move.x);
                animator.SetFloat("MoveY", move.y);

                if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
                    spriteRenderer.flipX = move.x < 0;
                else
                    spriteRenderer.flipX = false;
            }

            // 발소리 재생중이 아니면 재생 시작
            if (!audioSource.isPlaying && footstepSound != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (!anotherAnim)
            {
                animator.enabled = false;
                UpdateIdleSprite();
            }

            // 멈췄으면 발소리 정지
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    public GameObject target;
    public bool goal = false;

    void FixedUpdate()
    {
        if (canMove)
        {
            transform.Translate(move * moveSpeed * Time.fixedDeltaTime);
            goal = false;
        }
        else
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.1f);

                if ((transform.position - target.transform.position).sqrMagnitude < 0.0001f)
                {
                    goal = true;
                }
            }
        }
    }

    void UpdateIdleSprite()
    {
        if (Mathf.Abs(lastMoveX) > Mathf.Abs(lastMoveY))
        {
            spriteRenderer.sprite = idleSide;
            spriteRenderer.flipX = lastMoveX < 0;
        }
        else
        {
            spriteRenderer.flipX = false;

            if (lastMoveY > 0)
                spriteRenderer.sprite = idleBack;
            else
                spriteRenderer.sprite = idleFront;
        }
    }

    public bool animCheck = false;

    public void CallAnimation(string animName, float speed = 1f)
    {
        if (!animCheck)
        {
            anotherAnim = true;
            canMove = false;
            if (!animator.enabled)
                animator.enabled = true;

            if (animName == "Climb")
                GetComponent<BoxCollider2D>().enabled = false;

            animator.SetTrigger(animName);
            Debug.Log(animName);

            animCheck = true;
        }

        if (animName == "Climb")
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    public void RetunAnimation()
    {
        animator.SetTrigger("Return");
        GetComponent<BoxCollider2D>().enabled = true;
        anotherAnim = false;
        animCheck = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            transform.position = collision.collider.GetComponent<TileWalk>().StepTile(transform);
        }
    }
}
