using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("Chỉ số")]
    public int hp = 60; // Máu tối đa

    [Header("Bắn")]
    public GameObject enemyBulletPrefab;
    public float minFireRate = 3f; // Thời gian giữa các lần bắn nhanh nhất
    public float maxFireRate = 6f; // Thời gian giữa các lần bắn chậm nhất
    private float fireRate;
    private float nextFireTime;
    [Range(0f, 1f)] public float shootChance = 0.4f; // Xác suất bắn

    [Header("Hiệu ứng chết")]
    public GameObject explosionPrefab;

    [Header("Item rơi")]
    public GameObject powerUpPrefab;
    [Range(0f, 1f)] public float dropChancePowerUp = 0.3f;
    public GameObject drumstickPrefab;
    [Range(0f, 1f)] public float dropChanceDrumstick = 0.5f;

    void Start()
    {
        // Mỗi enemy có tốc độ bắn riêng
        fireRate = Random.Range(minFireRate, maxFireRate);
        nextFireTime = Time.time + Random.Range(1f, fireRate);
    }

    void Update()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.y < max.y && transform.position.y > min.y)
        {
            if (Time.time >= nextFireTime)
            {
                if (Random.value <= shootChance)
                {
                    Shoot();
                    // Sau mỗi lần bắn, bắn chậm hơn một chút
                    fireRate = Mathf.Min(fireRate + 0.5f, 8f);
                }
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    public void Shoot()
    {
        if (enemyBulletPrefab != null)
            Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0) Die();
    }

    void Die()
    {
        PlayExplosion();
        TryDropItems();
        Destroy(gameObject);
    }

    void PlayExplosion()
    {
        if (explosionPrefab != null)
        {
            GameObject expl = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(expl, 0.5f);
        }
    }

    void TryDropItems()
    {
        if (powerUpPrefab != null && Random.value <= dropChancePowerUp)
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);

        if (drumstickPrefab != null && Random.value <= dropChanceDrumstick)
        {
            GameObject drumstick = Instantiate(drumstickPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = drumstick.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1;
                rb.AddForce(Vector2.right * Random.Range(-1f, 1f), ForceMode2D.Impulse);
            }
        }
    }
}