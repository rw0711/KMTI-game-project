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
        Climb(); // 사다리 움직임
    }

    void Move()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");

        // 사다리 중일 땐 좌우 이동 제한 (원하면 허용 가능)
        if (!isClimbing)
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

    void Climb()
    {
        if (isClimbing)
        {
            float v = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0, v * climbSpeed);
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 3f; // 원래 중력값
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
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
