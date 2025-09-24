using UnityEngine;
using System.Collections;
public class Move : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groudlayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    private bool isGrounded;
    private bool isAttack = false;

    private Animator animator;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimation();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groudlayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;

        animator.SetBool("isRuning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            StartCoroutine(Attack1());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone") || collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            playerHealth.TakeDamage();
        }
    }

    IEnumerator Attack1()
    {
        animator.SetTrigger("Attack");
        isAttack = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        Shoot();

        yield return new WaitForSeconds(4f / 6f);

        isAttack = false;
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        rbBullet.linearVelocity = new Vector2(direction * bulletSpeed, 0f);
        float rotationZ = direction > 0 ? -90f : 90f;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

    }
}