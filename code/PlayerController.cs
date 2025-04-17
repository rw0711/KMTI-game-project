using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위한 네임스페이스

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float climbSpeed = 4f;

    private bool isGrounded;
    private bool isClimbing = false;
    private bool canClimb = false; // 사다리 영역에 있는지 여부
    private bool isCrouching = false; // 앉은 상태

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Vector3 originalScale;

    public Sprite standingSprite; // 서있는 상태 스프라이트
    public Sprite crouchingSprite; // 앉은 상태 스프라이트
    private SpriteRenderer spriteRenderer;

    public float crouchScaleFactor = 0.5f; // 앉았을 때 크기 비율 (50%로 설정됨)

    private bool isNearPortal = false; // 포탈 근처에 있는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 추가
        originalScale = transform.localScale;
    }

    void Update()
    {
        Move();
        Jump();
        Crouch();
        ClimbControl(); // 사다리 시작/종료 조건 처리

        if (isNearPortal && Input.GetKeyDown(KeyCode.F)) // F 키 입력 시
        {
            // 씬 전환 코드 추가 (다른 씬으로 이동)
            LoadNewScene("NewSceneName"); // "NewSceneName"을 실제 이동하고 싶은 씬 이름으로 바꿔주세요
        }
    }

    void FixedUpdate()
    {
        Climb(); // 실제 사다리 이동
    }

    void Move()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");

        if (!isClimbing)
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection != 0)
        {
            // 좌우 방향키에 따라 캐릭터의 방향을 반전
            // Mathf.Sign을 사용하여 moveDirection에 따라 반전 여부 결정
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
            // 앉은 상태로 스프라이트 변경
            spriteRenderer.sprite = crouchingSprite;
            // 앉을 때 크기 변경
            transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScaleFactor, originalScale.z);
            isCrouching = true;
        }
        else if (!isClimbing)
        {
            // 서 있는 상태로 스프라이트 변경
            spriteRenderer.sprite = standingSprite;
            // 서 있을 때 크기 원래대로 변경
            transform.localScale = originalScale;
            isCrouching = false;
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

        if (collision.CompareTag("Portal")) // 포탈과 충돌 시
        {
            isNearPortal = true; // 포탈 근처로 감지
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            canClimb = false;
        }

        if (collision.CompareTag("Portal")) // 포탈을 벗어날 때
        {
            isNearPortal = false; // 포탈 근처에서 벗어났을 때
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

    // 씬 전환 함수
    void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 이름으로 전환
    }
}
