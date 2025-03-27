using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float climbSpeed = 4f;

    private bool isGrounded;
    private bool isClimbing = false;
    private bool canClimb = false; // 사다리 영역에 있는지 여부

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        Move();
        Jump();
        Crouch();
        ClimbControl(); // 사다리 시작/종료 조건 처리
    }

    void FixedUpdate()
    {
        Climb(); // 실제 사다리 이동
    }

    void Move()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");

        // 사다리 중에도 좌우 이동 가능
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveDirection) * originalScale.x, originalScale.y, originalScale.z);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.DownArrow) && !isClimbing)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        }
        else if (!isClimbing)
        {
            transform.localScale = originalScale;
        }
    }

    void ClimbControl()
    {
        // ↑ 키를 누른 상태에서만 사다리 타기 시작
        if (canClimb && Input.GetKey(KeyCode.UpArrow))
        {
            isClimbing = true;
        }

        // ✅ 좌우 방향 입력 시 사다리 상태 해제
        if (isClimbing && Input.GetAxisRaw("Horizontal") != 0)
        {
            isClimbing = false;
        }

        // 사다리에서 내려왔거나 사다리 영역 벗어나면 종료
        if (!canClimb)
        {
            isClimbing = false;
        }
    }

    void Climb()
    {
        if (isClimbing)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0f, vertical * climbSpeed);
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            canClimb = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
