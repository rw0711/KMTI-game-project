using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private bool isGrounded;
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
    }

    void Move()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveDirection) * originalScale.x, originalScale.y, originalScale.z);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; // 점프 후 공중 상태로 변경
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    // ✅ 지면 감지 (OnCollisionEnter2D & OnCollisionExit2D 사용)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 땅에 닿았을 때 점프 가능
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 땅에서 떨어지면 점프 불가
        }
    }
}
