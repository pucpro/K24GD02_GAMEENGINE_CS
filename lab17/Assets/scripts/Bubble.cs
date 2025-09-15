using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bubble : MonoBehaviour
{
    public int colorId;
    private BoardManager board;
    private Rigidbody2D rb;
    [HideInInspector] public int row, col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void Init(BoardManager bm)
    {
        board = bm;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.WakeUp();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Lấy contact point đầu tiên
        ContactPoint2D cp = col.contacts[0];

        // 1) Va chạm với tường → bounce bằng AddForce
        if (col.collider.CompareTag("Wall"))
        {
            float speed = rb.linearVelocity.magnitude;
            Vector2 incomingDir = rb.linearVelocity.normalized;
            Vector2 reflectedDir = Vector2.Reflect(incomingDir, cp.normal);

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(reflectedDir * speed, ForceMode2D.Impulse);
            return;
        }

        // 2) Va chạm với bubble khác → snap vào grid
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            var hitBubble = col.collider.GetComponent<Bubble>();
            if (hitBubble != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;

                board.SnapBubble(this, hitBubble, cp.normal);

                // Cho phép shooter bắn tiếp
                var shooter = Object.FindAnyObjectByType<ShooterController>();
                shooter?.OnBulletSettled();
            }
        }
    }
}