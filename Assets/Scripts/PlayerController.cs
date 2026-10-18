using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invulnerabilityTime = 0.8f;

    [Header("Dash")]
    [SerializeField] private float dashDuration = 0.35f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int currentHealth;

    private bool isGrounded = true;
    private bool isDashing;
    private bool isInvulnerable;
    private bool isDead;

    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (animator != null)
            animator.SetBool("Grounded", true);

        if (GameManager.Instance != null)
            GameManager.Instance.SetHealth(currentHealth, maxHealth);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !isGrounded || isDead)
            return;

        rb.linearVelocity =
            new Vector2(rb.linearVelocity.x, jumpForce);

        isGrounded = false;

        if (animator != null)
            animator.SetBool("Grounded", false);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || isDashing || isDead)
            return;

        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;

        if (GameManager.Instance != null)
            GameManager.Instance.SetDash(true);

        yield return new WaitForSeconds(dashDuration);

        if (GameManager.Instance != null)
            GameManager.Instance.SetDash(false);

        isDashing = false;
    }

    public void TakeDamage()
    {
        if (isInvulnerable || isDead)
            return;

        currentHealth--;

        if (GameManager.Instance != null)
            GameManager.Instance.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
            animator.SetTrigger("Hit");

        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        isInvulnerable = true;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(
            Mathf.Max(0f, invulnerabilityTime - 0.2f)
        );

        isInvulnerable = false;
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Death");

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }

    public void AddScore(int amount)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(amount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (animator != null && !isDead)
                animator.SetBool("Grounded", true);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.Instance != null)
        {
            GameManager.Instance.TogglePause();
        }
    }
}