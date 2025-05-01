using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 3f;
    Vector3 move;

    SpriteRenderer spriteRenderer;
    Animator animator;

    public Sprite idleFront;
    public Sprite idleBack;
    public Sprite idleSide;

    float lastMoveX = 0;
    float lastMoveY = -1;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        // 🔽 여기서 실제 이동 중인지 확인
        isMoving = move.magnitude > 0;

        if (isMoving)
        {
            animator.enabled = true;
            animator.SetFloat("MoveX", move.x);
            animator.SetFloat("MoveY", move.y);

            // 이동 중에도 flipX 처리
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                spriteRenderer.flipX = move.x < 0;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            animator.enabled = false;
            UpdateIdleSprite(); // 멈췄을 때 idle 이미지 설정
        }
    }




    void FixedUpdate()
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }

    void UpdateIdleSprite()
    {
        // 마지막 방향 기준으로 정지 이미지 설정
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
}
